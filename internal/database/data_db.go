package database

import (
	"cmd/reporting-api/internal/models"
	"fmt"
	"time"

	log "krr-app-gitlab01.europe.mittalco.com/pait/modules/go/logging"
)

func HandleSingleAlias(alias string) (*models.PathDataResponse, error) {
	now := time.Now()

	var cat models.Category
	if err := DB.Where("\"Alias\" = ? AND \"Visible\" = true", alias).First(&cat).Error; err == nil {
		return FetchVisibleDataFlat()
	}

	if err := DB.Where("\"PrivateAlias\" = ? AND \"PrivateAliasExpiresAt\" > ? AND \"Visible\" = true",
		alias, now).First(&cat).Error; err == nil {
		if cat.Id == nil {
			return nil, fmt.Errorf("unexpected nil Id in private category record")
		}
		return getCategorySubtree(*cat.Id)
	}

	var repWithParent models.ReportWithParent
	if err := DB.
		Table("\"sys-reporting\".\"Reports\" AS r").
		Joins("LEFT JOIN \"sys-reporting\".\"CategoryReports\" AS cr ON r.\"Id\" = cr.\"ReportsId\"").
		Joins("LEFT JOIN \"sys-reporting\".\"Categories\" AS c ON cr.\"CategoriesId\" = c.\"Id\"").
		Where("r.\"PrivateAlias\" = ? AND r.\"PrivateAliasExpiresAt\" > ? AND r.\"Visible\" = true", alias, now).
		Select("r.*", "c.\"Id\" AS \"ParentId\"").
		First(&repWithParent).Error; err == nil {

		rep := repWithParent.Report
		rep.ParentID = repWithParent.ParentID
		rep.Type = "file"
		rep.Data.URL = rep.URL

		return &models.PathDataResponse{
			Categories: []models.GetCategory{},
			Reports:    []models.Report{rep},
			IsPrivate:  true,
		}, nil
	}

	return FetchVisibleDataFlat()
}

func getCategorySubtree(rootID int) (*models.PathDataResponse, error) {
	var ids []int

	cte := `
		WITH RECURSIVE cat_tree AS (
		    SELECT "Id"
		    FROM "sys-reporting"."Categories"
		    WHERE "Id" = ? AND "Visible" = true
		    UNION ALL
		    SELECT c."Id"
		    FROM "sys-reporting"."Categories" c
		    JOIN cat_tree ct ON c."ParentId" = ct."Id"
		    WHERE c."Visible" = true
		)
		SELECT "Id" FROM cat_tree
	`

	if err := DB.Raw(cte, rootID).Pluck("Id", &ids).Error; err != nil {
		return nil, fmt.Errorf("failed to get category ids: %v", err)
	}

	if len(ids) == 0 {
		return &models.PathDataResponse{
			Categories: []models.GetCategory{},
			Reports:    []models.Report{},
			IsPrivate:  true,
		}, fmt.Errorf("category subtree not found for rootID=%d", rootID)
	}

	var categories []models.GetCategory
	if err := DB.
		Where("\"Id\" IN ?", ids).
		Find(&categories).Error; err != nil {
		return nil, fmt.Errorf("failed to load categories: %v", err)
	}

	for i := range categories {
		categories[i].Type = "folder"
	}

	var reportsWithParent []models.ReportWithParent
	if err := DB.
		Table("\"sys-reporting\".\"Reports\" AS r").
		Joins("JOIN \"sys-reporting\".\"CategoryReports\" AS cr ON r.\"Id\" = cr.\"ReportsId\"").
		Joins("LEFT JOIN \"sys-reporting\".\"Categories\" AS c ON cr.\"CategoriesId\" = c.\"Id\"").
		Where("cr.\"CategoriesId\" IN ? AND r.\"Visible\" = true", ids).
		Select("r.*", "c.\"Id\" AS \"ParentId\"").
		Find(&reportsWithParent).Error; err != nil {
		return nil, fmt.Errorf("failed to load reports: %v", err)
	}

	var reports []models.Report
	for _, rwp := range reportsWithParent {
		r := rwp.Report
		r.ParentID = rwp.ParentID
		r.Type = "file"
		r.Data.URL = r.URL
		reports = append(reports, r)
	}

	return &models.PathDataResponse{
		Categories: categories,
		Reports:    reports,
		IsPrivate:  true,
	}, nil
}

func FetchVisibleDataFlat() (*models.PathDataResponse, error) {
	var categories []models.GetCategory

	err := DB.Raw(`
	    WITH RECURSIVE cat_tree AS (
	        SELECT c.*
	        FROM "sys-reporting"."Categories" c
	        JOIN "sys-reporting"."CategoryReports" cr ON cr."CategoriesId" = c."Id"
	        JOIN "sys-reporting"."Reports" r ON cr."ReportsId" = r."Id"
	        WHERE r."OperationName" = 'public' AND r."Visible" = true AND c."Visible" = true
	
	        UNION
	
	        SELECT parent.*
	        FROM "sys-reporting"."Categories" parent
	        JOIN cat_tree ct ON ct."ParentId" = parent."Id"
	        WHERE parent."Visible" = true
	    )
	    SELECT DISTINCT *
	    FROM cat_tree
	`).Scan(&categories).Error

	if err != nil {
		log.Error(fmt.Sprintf("failed to query categories: %v", err))
		return nil, err
	}

	var categoryIds = make([]int, 0, len(categories))
	for i := range categories {
		categories[i].Type = "folder"
		if categories[i].Id != nil {
			categoryIds = append(categoryIds, *categories[i].Id)
		}
	}

	var reports []models.Report
	if len(categoryIds) > 0 {
		var reportsWithParent []models.ReportWithParent
		if err := DB.Table("\"sys-reporting\".\"Reports\" AS r").
			Joins("JOIN \"sys-reporting\".\"CategoryReports\" cr ON r.\"Id\" = cr.\"ReportsId\"").
			Where("r.\"Visible\" = true AND cr.\"CategoriesId\" IN ?", categoryIds).
			Select("r.*", "cr.\"CategoriesId\" AS \"ParentId\"").
			Find(&reportsWithParent).Error; err != nil {
			return nil, fmt.Errorf("error loading reports: %v", err)
		}

		reports = make([]models.Report, len(reportsWithParent))
		for i, rwp := range reportsWithParent {
			r := rwp.Report
			r.ParentID = rwp.ParentID
			r.Type = "file"
			r.Data.URL = r.URL
			reports[i] = r
		}
	}

	return &models.PathDataResponse{
		Categories: categories,
		Reports:    reports,
		IsPrivate:  false,
	}, nil
}
