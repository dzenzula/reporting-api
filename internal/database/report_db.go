package database

import (
	"cmd/reporting-api/internal/config"
	"cmd/reporting-api/internal/models"
	"fmt"
	"regexp"
	"slices"
	"strings"
	"time"

	"gorm.io/gorm"
	auth "krr-app-gitlab01.europe.mittalco.com/pait/modules/go/authorization"
	log "krr-app-gitlab01.europe.mittalco.com/pait/modules/go/logging"
)

const (
	errRunQuery string = "error running query: %s"
)

func FetchAllFavoriteReports() ([]models.FavoriteReportItem, error) {
	var favoriteReports []models.FavoriteReportItem

	err := DB.Table("\"sys-reporting\".\"FavoriteReports\" AS fr").
		Select(`
		r."Id",
		r."Text",
		r."Description",
		r."Alias",
		fr."CategoryId" AS "ParentId",
		c."Alias" AS "ParentAlias"
	`).
		Joins(`JOIN "sys-reporting"."Reports" r ON r."Id" = fr."ReportId"`).
		Joins(`LEFT JOIN "sys-reporting"."Categories" c ON c."Id" = fr."CategoryId"`).
		Where(`fr."Login" = ?`, auth.GetUserMail()).
		Scan(&favoriteReports).Error

	if err != nil {
		log.Error(fmt.Sprintf(errRunQuery, err))
		return nil, err
	}

	return favoriteReports, nil
}

func AddFavoriteReport(dto models.FavoriteReportDTO) error {
	var category models.Category
	if err := DB.Model(&models.Category{}).First(&category, "\"Id\" = ?", dto.CategoryId).Error; err != nil {
		if err == gorm.ErrRecordNotFound {
			msg := fmt.Sprintf("category with this Id not found: %d", dto.CategoryId)
			log.Error(msg)
			return fmt.Errorf(msg)
		} else {
			log.Error(fmt.Sprintf(errRunQuery, err))
			return err
		}
	}

	var report models.Report
	if err := DB.Model(&models.Report{}).First(&report, "\"Id\" = ?", dto.ReportId).Error; err != nil {
		if err == gorm.ErrRecordNotFound {
			msg := fmt.Sprintf("report with this Id not found: %d", dto.ReportId)
			log.Error(msg)
			return fmt.Errorf(msg)
		} else {
			log.Error(fmt.Sprintf(errRunQuery, err))
			return err
		}
	}

	if !report.Visible {
		msg := fmt.Sprintf("report %d is not accessible", dto.ReportId)
		log.Error(msg)
		return fmt.Errorf(msg)
	}

	var count int64
	if err := DB.Table("\"sys-reporting\".\"CategoryReports\"").
		Where("\"CategoriesId\" = ? AND \"ReportsId\" = ?", dto.CategoryId, dto.ReportId).
		Count(&count).Error; err != nil {
		log.Error(fmt.Sprintf(errRunQuery, err))
		return err
	}

	if count == 0 {
		msg := fmt.Sprintf("report %d does not belong to category %d", dto.ReportId, dto.CategoryId)
		log.Error(msg)
		return fmt.Errorf(msg)
	}

	var login string = auth.GetUserMail()

	var existingFavorite models.FavoriteReport
	if err := DB.Model(&models.FavoriteReport{}).
		Where("\"ReportId\" = ? AND \"CategoryId\" = ? AND \"Login\" = ?", dto.ReportId, dto.CategoryId, login).
		First(&existingFavorite).Error; err == nil {
		msg := "this report in this category is already in favorites"
		log.Error(msg)
		return fmt.Errorf(msg)
	}

	newFavReport := models.FavoriteReport{
		ReportId:   dto.ReportId,
		CategoryId: dto.CategoryId,
		Login:      login,
	}

	if err := DB.Model(&models.FavoriteReport{}).Create(&newFavReport).Error; err != nil {
		log.Error(fmt.Sprintf("error creating favorite report: %v", err))
		return err
	}

	return nil
}

func RemoveFavoriteReport(dto models.FavoriteReportDTO) error {
	login := auth.GetUserMail()

	var report models.Report
	if err := DB.Model(&models.Report{}).First(&report, "\"Id\" = ?", dto.ReportId).Error; err != nil {
		if err == gorm.ErrRecordNotFound {
			msg := fmt.Sprintf("report with this Id not found: %d", dto.ReportId)
			log.Error(msg)
			return fmt.Errorf(msg)
		} else {
			log.Error(fmt.Sprintf(errRunQuery, err))
			return err
		}
	}

	var category models.Category
	if err := DB.Model(&models.Category{}).First(&category, "\"Id\" = ?", dto.CategoryId).Error; err != nil {
		if err == gorm.ErrRecordNotFound {
			msg := fmt.Sprintf("category with this Id not found: %d", dto.CategoryId)
			log.Error(msg)
			return fmt.Errorf(msg)
		} else {
			log.Error(fmt.Sprintf(errRunQuery, err))
			return err
		}
	}

	var count int64
	if err := DB.Table("\"sys-reporting\".\"CategoryReports\"").
		Where("\"CategoriesId\" = ? AND \"ReportsId\" = ?", dto.CategoryId, dto.ReportId).
		Count(&count).Error; err != nil {
		log.Error(fmt.Sprintf(errRunQuery, err))
		return err
	}

	if count == 0 {
		msg := fmt.Sprintf("report %d does not belong to category %d", dto.ReportId, dto.CategoryId)
		log.Error(msg)
		return fmt.Errorf(msg)
	}

	var favoriteReport models.FavoriteReport
	if err := DB.Model(&models.FavoriteReport{}).
		Where("\"Login\" = ? AND \"ReportId\" = ? AND \"CategoryId\" = ?", login, dto.ReportId, dto.CategoryId).
		First(&favoriteReport).Error; err != nil {
		if err == gorm.ErrRecordNotFound {
			msg := "this report is NOT in favorites"
			log.Error(msg)
			return fmt.Errorf(msg)
		} else {
			log.Error(fmt.Sprintf(errRunQuery, err))
			return err
		}
	}

	if err := DB.Model(&models.FavoriteReport{}).
		Where("\"Login\" = ? AND \"ReportId\" = ? AND \"CategoryId\" = ?", login, dto.ReportId, dto.CategoryId).
		Delete(&models.FavoriteReport{}).Error; err != nil {
		msg := fmt.Sprintf("failed to delete favorite report: %v", err)
		log.Error(msg)
		return fmt.Errorf(msg)
	}

	return nil
}

func GetLastVisitedReport(quantity int) ([]models.VisitedReport, error) {
	var reports []models.VisitedReport

	err := DB.Table("\"sys-reporting\".\"VisitHistory\" AS vh").
		Select(`
			r."Text" AS "Name",
			r."Alias" AS "Alias",
			c."Alias" AS "CategoryAlias"
		`).
		Joins(`
			JOIN (
				SELECT "ReportId", "CategoryId", MAX("Dt") AS max_dt
        		FROM "sys-reporting"."VisitHistory"
        		WHERE "Login" = ?
        		GROUP BY "ReportId", "CategoryId"
      		) latest ON latest."ReportId" = vh."ReportId" 
              		AND latest."CategoryId" = vh."CategoryId"
              		AND latest.max_dt = vh."Dt"
		`, auth.GetUserMail()).
		Joins(`JOIN "sys-reporting"."Reports" AS r ON r."Id" = vh."ReportId"`).
		Joins(`JOIN "sys-reporting"."Categories" AS c ON c."Id" = vh."CategoryId"`).
		Where("r.\"Visible\" = ?", true).
		Order("latest.max_dt DESC").
		Limit(quantity).
		Scan(&reports).Error

	if err != nil {
		return nil, fmt.Errorf("error fetching visited reports: %w", err)
	}

	return reports, nil
}

func AddVisitedReport(dto models.TrackVisitDTO, ip string) error {
	var categoryExists bool
	err := DB.
		Model(&models.Category{}).
		Select("count(1) > 0").
		Where(`"Id" = ?`, dto.CategoryId).
		Find(&categoryExists).Error

	if err != nil {
		log.Error(fmt.Sprintf(errRunQuery, err))
		return err
	}

	if !categoryExists {
		msg := fmt.Sprintf("category with this Id not found: %d", dto.CategoryId)
		log.Error(msg)
		return fmt.Errorf(msg)
	}

	var reportExists bool
	err = DB.
		Model(&models.Report{}).
		Select("count(1) > 0").
		Where(`"Id" = ?`, dto.ReportId).
		Find(&reportExists).Error

	if err != nil {
		log.Error(fmt.Sprintf(errRunQuery, err))
		return err
	}

	if !reportExists {
		msg := fmt.Sprintf("report with this Id not found: %d", dto.ReportId)
		log.Error(msg)
		return fmt.Errorf(msg)
	}

	visitedReport := models.VisitHistory{
		ReportId:   dto.ReportId,
		CategoryId: dto.CategoryId,
		Login:      auth.GetUserMail(),
		IpAddress:  ip,
	}

	if err := DB.Model(&models.VisitHistory{}).Create(&visitedReport).Error; err != nil {
		log.Error(fmt.Sprintf("error add visited report: %v", err))
		return err
	}

	return nil
}

func FetchAllReportsForAdmin() ([]models.Report, error) {
	var reportsWithParent []models.ReportWithParent
	err := DB.Table("\"sys-reporting\".\"Reports\" AS r").
		Joins("LEFT JOIN \"sys-reporting\".\"CategoryReports\" cr ON r.\"Id\" = cr.\"ReportsId\"").
		Joins("LEFT JOIN \"sys-reporting\".\"Categories\" c ON cr.\"CategoriesId\" = c.\"Id\"").
		Select("r.*, c.\"Id\" AS \"ParentId\"").
		Find(&reportsWithParent).Error

	if err != nil {
		msg := fmt.Sprintf("failed to query reports: %v", err)
		log.Error(msg)
		return nil, fmt.Errorf(msg)
	}

	var reports []models.Report
	for _, rwp := range reportsWithParent {
		r := rwp.Report
		r.ParentID = rwp.ParentID
		r.Type = "file"
		r.Data.URL = r.URL
		reports = append(reports, r)
	}

	return reports, nil
}

func FetchAllReports() ([]models.Report, error) {
	permissions := auth.GetPermissions()
	isAdmin := slices.Contains(permissions, config.GlobalConfig.Permissions.AdminAccess)
	var err error

	var reportsWithParent []models.ReportWithParent

	if isAdmin {
		err = DB.Table("\"sys-reporting\".\"Reports\" AS r").
			Joins("LEFT JOIN \"sys-reporting\".\"CategoryReports\" cr ON r.\"Id\" = cr.\"ReportsId\"").
			Joins("LEFT JOIN \"sys-reporting\".\"Categories\" c ON cr.\"CategoriesId\" = c.\"Id\"").
			Select("r.*, c.\"Id\" AS \"ParentId\"").
			Find(&reportsWithParent).Error
	} else {
		err = DB.Table("\"sys-reporting\".\"Reports\" AS r").
			Joins("LEFT JOIN \"sys-reporting\".\"CategoryReports\" cr ON r.\"Id\" = cr.\"ReportsId\"").
			Joins("LEFT JOIN \"sys-reporting\".\"Categories\" c ON cr.\"CategoriesId\" = c.\"Id\"").
			Where("r.\"OperationName\" = ? OR r.\"OperationName\" IN (?)", "public", permissions).
			Select("r.*", "c.\"Id\" AS \"ParentId\"").
			Find(&reportsWithParent).Error
	}

	if err != nil {
		msg := fmt.Sprintf("failed to query reports: %v", err)
		log.Error(msg)
		return nil, fmt.Errorf(msg)
	}

	var reports []models.Report
	for _, rwp := range reportsWithParent {
		r := rwp.Report
		r.ParentID = rwp.ParentID
		r.Type = "file"
		r.Data.URL = r.URL
		reports = append(reports, r)
	}

	return reports, nil
}

func UpdateReport(uRep models.UpdateReport) error {
	if strings.TrimSpace(uRep.Text) == "" {
		return fmt.Errorf("Name cannot be empty")
	}

	privateAliasTrimmed := strings.TrimSpace(uRep.PrivateAlias)
	expiresAtTrimmed := strings.TrimSpace(uRep.PrivateAliasExpiresAt)

	var parsedExpiresAt *time.Time
	if expiresAtTrimmed != "" {
		parsedDate, err := time.Parse("2006-01-02", expiresAtTrimmed)
		if err != nil {
			return fmt.Errorf("Invalid date format, use YYYY-MM-DD")
		}

		tomorrow := time.Now().AddDate(0, 0, 1).Truncate(24 * time.Hour)
		if parsedDate.Before(tomorrow) {
			return fmt.Errorf("Expiration date must be from tomorrow or later")
		}

		parsedExpiresAt = &parsedDate
	}

	if expiresAtTrimmed != "" && privateAliasTrimmed == "" {
		return fmt.Errorf("Private alias is required if expiration date is specified")
	}

	var currentReport models.Report
	if err := DB.Where("\"Id\" = ?", uRep.Id).First(&currentReport).Error; err != nil {
		msg := fmt.Sprintf("report with this id %d does not exist", uRep.Id)
		log.Error(msg)
		return fmt.Errorf(msg)
	}

	if privateAliasTrimmed != "" {
		if expiresAtTrimmed == "" {
			return fmt.Errorf("Set expiration date for Private Alias")
		}
		if len(privateAliasTrimmed) != 8 {
			return fmt.Errorf("Private Alias must be 8 characters")
		}

		if privateAliasTrimmed == currentReport.Alias {
			return fmt.Errorf("Private Alias cannot be the same as Alias of the report")
		}

		var validAlias = regexp.MustCompile(`^[a-zA-Z0-9]+$`)
		if !validAlias.MatchString(privateAliasTrimmed) {
			return fmt.Errorf("Private Alias must contain only letters and digits")
		}

		var duplicateCategory models.Category
		if err := DB.Where("\"Alias\" = ? OR \"PrivateAlias\" = ?", privateAliasTrimmed, privateAliasTrimmed).
			First(&duplicateCategory).Error; err == nil {
			msg := fmt.Sprintf("Private Alias already used as alias or private alias. Category name: %s", duplicateCategory.Text)
			log.Error(msg)
			return fmt.Errorf(msg)
		}

		var duplicateReport models.Report
		if err := DB.Where("(\"Alias\" = ? OR \"PrivateAlias\" = ?) AND \"Id\" != ?",
			privateAliasTrimmed, privateAliasTrimmed, uRep.Id).
			First(&duplicateReport).Error; err == nil {
			msg := fmt.Sprintf("Private Alias already used as alias or private alias. Report name: %s", duplicateReport.Text)
			log.Error(msg)
			return fmt.Errorf(msg)
		}
	}

	var duplicateByText models.Report
	if err := DB.
		Where("\"Text\" = ? AND \"Id\" != ?", uRep.Text, uRep.Id).
		First(&duplicateByText).Error; err == nil {
		msg := fmt.Sprintf("report with this name already exists. Report name: %s", duplicateByText.Text)
		log.Error(msg)
		return fmt.Errorf(msg)
	}

	var duplicateByURL models.Report
	if err := DB.
		Where("\"URL\" = ? AND \"Id\" != ?", uRep.URL, uRep.Id).
		First(&duplicateByURL).Error; err == nil {
		msg := fmt.Sprintf("report with this URL already exists. Report name: %s", duplicateByURL.Text)
		log.Error(msg)
		return fmt.Errorf(msg)
	}

	isMail, err := auth.CheckUserMail(uRep.Owner)
	if !isMail {
		if err != nil {
			log.Error(err.Error())
			return err
		}
		msg := "e-mail not found"
		log.Error(msg)
		return fmt.Errorf(msg)
	}

	updateData := map[string]interface{}{
		"Text":                  uRep.Text,
		"Description":           uRep.Description,
		"Owner":                 strings.ToLower(strings.TrimSpace(uRep.Owner)),
		"Visible":               uRep.Visible,
		"URL":                   uRep.URL,
		"OperationName":         strings.ToLower(strings.TrimSpace(uRep.OperationName)),
		"PrivateAlias":          privateAliasTrimmed,
		"PrivateAliasExpiresAt": parsedExpiresAt,
		"UpdatedAt":             time.Now(),
		"UpdatedBy":             auth.ReturnDomainUser(),
	}

	if err := DB.Model(&models.Report{}).Where("\"Id\" = ?", uRep.Id).Updates(updateData).Error; err != nil {
		log.Error(fmt.Sprintf("failed to update report: %v", err))
		return err
	}

	return nil
}

func checkReportInCategory(reportID, categoryID int) (bool, error) {
	var catReport models.CategoryReports
	err := DB.Where("\"ReportsId\" = ? AND \"CategoriesId\" = ?", reportID, categoryID).First(&catReport).Error
	if err != nil {
		if err == gorm.ErrRecordNotFound {
			return false, nil
		}
		return false, err
	}
	return true, nil
}

func moveReportToCategory(reportID, fromCategoryID, toCategoryID int) error {
	err := DB.Where("\"ReportsId\" = ? AND \"CategoriesId\" = ?", reportID, fromCategoryID).Delete(&models.CategoryReports{}).Error
	if err != nil {
		return fmt.Errorf("error deleting report from category: %v", err)
	}

	newCatReport := models.CategoryReports{
		ReportsId:    reportID,
		CategoriesId: toCategoryID,
	}

	err = DB.Create(&newCatReport).Error
	if err != nil {
		return fmt.Errorf("error adding report to category: %v", err)
	}

	return nil
}

func UpdateCategoryReport(uCatRep models.UpdateCategoryParent) error {
	exists, err := checkReportInCategory(uCatRep.Id, uCatRep.FromCat)
	if err != nil {
		log.Error(err.Error())
		return err
	}
	if !exists {
		msg := fmt.Sprintf("report with ID %d does not exist in the specified category %d", uCatRep.Id, uCatRep.FromCat)
		log.Error(msg)
		return fmt.Errorf(msg)
	}

	exists, err = checkReportInCategory(uCatRep.Id, uCatRep.ToCat)
	if err != nil {
		log.Error(err.Error())
		return err
	}
	if exists {
		msg := fmt.Sprintf("report with ID %d already exists in the specified category %d", uCatRep.Id, uCatRep.ToCat)
		log.Error(msg)
		return fmt.Errorf(msg)
	}

	err = moveReportToCategory(uCatRep.Id, uCatRep.FromCat, uCatRep.ToCat)
	if err != nil {
		log.Error(err.Error())
		return err
	}

	return nil
}

func AddReportRelation(reportID, categoryID int) error {
	var report models.Report
	if err := DB.Where("\"Id\" = ?", reportID).First(&report).Error; err != nil {
		if err == gorm.ErrRecordNotFound {
			return fmt.Errorf("report with ID %d does not exist", reportID)
		}
		return fmt.Errorf("error checking report existence: %v", err)
	}

	var category models.Category
	if err := DB.Where("\"Id\" = ?", categoryID).First(&category).Error; err != nil {
		if err == gorm.ErrRecordNotFound {
			return fmt.Errorf("category with ID %d does not exist", categoryID)
		}
		return fmt.Errorf("error checking category existence: %v", err)
	}

	var catRep models.CategoryReports
	err := DB.Where(&models.CategoryReports{ReportsId: reportID, CategoriesId: categoryID}).First(&catRep).Error
	if err == nil {
		return fmt.Errorf("relation between report %d and category %d already exists", reportID, categoryID)
	} else if err != gorm.ErrRecordNotFound {

		return fmt.Errorf("error checking existing relation: %v", err)
	}

	newCatRep := models.CategoryReports{
		ReportsId:    reportID,
		CategoriesId: categoryID,
	}

	if err := DB.Create(&newCatRep).Error; err != nil {
		return fmt.Errorf("failed to add report relation: %v", err)
	}

	return nil
}

func CreateReport(report models.CreateReport) (*int, error) {
	isMail, err := auth.CheckUserMail(report.Owner)
	if !isMail {
		if err != nil {
			log.Error(err.Error())
			return nil, err
		}
		msg := "e-mail not found"
		log.Error(msg)
		return nil, fmt.Errorf(msg)
	}

	if strings.TrimSpace(report.Text) == "" {
		return nil, fmt.Errorf("Name cannot be empty")
	}

	if strings.TrimSpace(report.Alias) == "" {
		return nil, fmt.Errorf("Alias cannot be empty")
	}

	var unknownCat models.Category
	if err := DB.Where("\"Id\" = ?", report.ParentId).First(&unknownCat).Error; err != nil {
		msg := fmt.Sprintf("category with this id %d does not exist", report.ParentId)
		log.Error(msg)
		return nil, fmt.Errorf(msg)
	}

	privateAliasTrimmed := strings.TrimSpace(report.PrivateAlias)
	expiresAtTrimmed := strings.TrimSpace(report.PrivateAliasExpiresAt)

	var parsedExpiresAt *time.Time
	if expiresAtTrimmed != "" {
		parsedDate, err := time.Parse("2006-01-02", expiresAtTrimmed)
		if err != nil {
			return nil, fmt.Errorf("Invalid date format, use YYYY-MM-DD")
		}

		tomorrow := time.Now().AddDate(0, 0, 1).Truncate(24 * time.Hour)
		if parsedDate.Before(tomorrow) {
			return nil, fmt.Errorf("Expiration date must be from tomorrow or later")
		}

		parsedExpiresAt = &parsedDate
	}

	if expiresAtTrimmed != "" && privateAliasTrimmed == "" {
		return nil, fmt.Errorf("Private alias is required if expiration date is specified")
	}

	if privateAliasTrimmed != "" {
		if expiresAtTrimmed == "" {
			return nil, fmt.Errorf("Set expiration date for Private Alias")
		}
		if len(privateAliasTrimmed) != 8 {
			return nil, fmt.Errorf("Private Alias must be 8 characters")
		}

		if privateAliasTrimmed == report.Alias {
			return nil, fmt.Errorf("Private Alias cannot be the same as Alias of the new report")
		}

		var validAlias = regexp.MustCompile(`^[a-zA-Z0-9]+$`)
		if !validAlias.MatchString(privateAliasTrimmed) {
			return nil, fmt.Errorf("Private Alias must contain only letters and digits")
		}

		var duplicateCategory models.Category
		if err := DB.Where("\"Alias\" = ? OR \"PrivateAlias\" = ?", privateAliasTrimmed, privateAliasTrimmed).
			First(&duplicateCategory).Error; err == nil {
			msg := fmt.Sprintf("Private Alias already used as alias or private alias. Category name: %s", duplicateCategory.Text)
			log.Error(msg)
			return nil, fmt.Errorf(msg)
		}

		var duplicateReport models.Report
		if err := DB.Where("\"Alias\" = ? OR \"PrivateAlias\" = ?", privateAliasTrimmed, privateAliasTrimmed).
			First(&duplicateReport).Error; err == nil {
			msg := fmt.Sprintf("Private Alias already used as alias or private alias. Report name: %s", duplicateReport.Text)
			log.Error(msg)
			return nil, fmt.Errorf(msg)
		}
	}

	var duplicateByText models.Report
	if err := DB.Where("\"Text\" = ?", report.Text).First(&duplicateByText).Error; err == nil {
		msg := fmt.Sprintf("report with this name already exists. Report name: %s", duplicateByText.Text)
		log.Error(msg)
		return nil, fmt.Errorf(msg)
	}

	var duplicateByAlias models.Report
	if err := DB.Where("\"Alias\" = ?", report.Alias).First(&duplicateByAlias).Error; err == nil {
		msg := fmt.Sprintf("report with this alias already exists. Report name: %s", duplicateByAlias.Text)
		log.Error(msg)
		return nil, fmt.Errorf(msg)
	}

	var duplicateByPrivateAlias models.Report
	if err := DB.Where("\"PrivateAlias\" = ?", report.Alias).First(&duplicateByPrivateAlias).Error; err == nil {
		msg := fmt.Sprintf("Alias already used as Private Alias in reports. Report name: %s", duplicateByPrivateAlias.Text)
		log.Error(msg)
		return nil, fmt.Errorf(msg)
	}

	var duplicateByPrivateAliasInCategories models.Category
	if err := DB.Where("\"PrivateAlias\" = ?", report.Alias).First(&duplicateByPrivateAliasInCategories).Error; err == nil {
		msg := fmt.Sprintf("Alias already used as Private Alias in categories. Category name: %s", duplicateByPrivateAliasInCategories.Text)
		log.Error(msg)
		return nil, fmt.Errorf(msg)
	}

	var duplicateByURL models.Report
	if err := DB.Where("\"URL\" = ?", report.URL).First(&duplicateByURL).Error; err == nil {
		msg := fmt.Sprintf("report with this URL already exists. Report name: %s", duplicateByURL.Text)
		log.Error(msg)
		return nil, fmt.Errorf(msg)
	}

	newReport := models.Report{
		Text:                  report.Text,
		URL:                   report.URL,
		Visible:               report.Visible,
		Alias:                 report.Alias,
		Description:           report.Description,
		OperationName:         report.OperationName,
		Owner:                 report.Owner,
		CreatedBy:             auth.ReturnDomainUser(),
		CreatedAt:             time.Now(),
		UpdatedBy:             auth.ReturnDomainUser(),
		PrivateAlias:          report.PrivateAlias,
		PrivateAliasExpiresAt: parsedExpiresAt,
	}

	if err := DB.Create(&newReport).Error; err != nil {
		msg := fmt.Sprintf("failed to create report: %v", err)
		log.Error(msg)
		return nil, fmt.Errorf(msg)
	}

	categoryReport := models.CategoryReports{
		ReportsId:    *newReport.Id,
		CategoriesId: report.ParentId,
	}
	if err := DB.Create(&categoryReport).Error; err != nil {
		msg := fmt.Sprintf("failed to create category report relation: %v", err)
		log.Error(msg)
		return nil, fmt.Errorf(msg)
	}

	return newReport.Id, nil
}

func RemoveReportById(reportID, categoryID int) error {
	var report models.Report
	if err := DB.Model(&report).Preload("Categories").Where("\"Reports\".\"Id\" = ?", reportID).First(&report).Error; err != nil {
		msg := fmt.Sprintf("report with ID %d not found: %s", reportID, err.Error())
		log.Error(msg)
		return fmt.Errorf(msg)
	}

	var category models.Category
	if err := DB.Where("\"Id\" = ?", categoryID).First(&category).Error; err != nil {
		msg := fmt.Sprintf("category with ID %d not found", categoryID)
		log.Error(msg)
		return fmt.Errorf(msg)
	}

	if err := DB.Where("\"ReportsId\" = ? AND \"CategoriesId\" = ?", reportID, categoryID).Delete(&models.CategoryReports{}).Error; err != nil {
		msg := fmt.Sprintf("failed to remove report from category: %v", err)
		log.Error(msg)
		return fmt.Errorf(msg)
	}

	/*if err := DB.Preload("\"sys-reporting\".\"Categories\"").Where("\"Id\" = ?", reportID).Delete(&category).Error; err != nil {
		msg := fmt.Sprintf("failed to remove report from category: %v", err)
		log.Error(msg)
		return fmt.Errorf(msg)
	}*/

	if len(report.Categories) == 1 {
		if err := DB.Where("\"Id\" = ?", reportID).Delete(&report).Error; err != nil {
			msg := fmt.Sprintf("failed to delete report: %v", err)
			log.Error(msg)
			return fmt.Errorf(msg)
		}
	}

	return nil
}
