package database

import (
	"cmd/reporting-api/internal/config"
	"cmd/reporting-api/internal/models"
	"fmt"
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

func FetchAllFavoriteReports() ([]models.Report, error) {
	var reportsWithParent []models.ReportWithParent
	err := DB.Table("\"sys-reporting\".\"FavoriteReports\" AS fr").
		Select("fr.*, r.*").
		Joins("JOIN \"sys-reporting\".\"Reports\" r ON r.\"Id\" = fr.\"ReportId\" AND r.\"Owner\" = fr.\"Login\"").
		Scan(&reportsWithParent).Error
	if err != nil {
		log.Error(fmt.Sprintf(errRunQuery, err))
		return nil, err
	}

	var favoriteReports []models.Report
	for _, rwp := range reportsWithParent {
		r := rwp.Report
		r.ParentID = rwp.ParentID
		r.Type = "file"
		r.Data.URL = r.URL
		favoriteReports = append(favoriteReports, r)
	}

	return favoriteReports, nil
}

func AddFavoriteReport(id int) error {
	var report models.Report
	if err := DB.Model(&models.Report{}).First(&report, "\"Id\" = ?", id).Error; err != nil {
		if err == gorm.ErrRecordNotFound {
			msg := fmt.Sprintf("report with this Id not found: %d", id)
			log.Error(msg)
			return fmt.Errorf(msg)
		} else {
			log.Error(fmt.Sprintf(errRunQuery, err))
			return err
		}
	}

	var favoriteReport models.FavoriteReport
	if err := DB.Model(&models.FavoriteReport{}).Where("\"ReportId\" = ?", id).First(&favoriteReport).Error; err == nil {
		msg := "this report already in favorite"
		log.Error(msg)
		return fmt.Errorf(msg)
	}

	newFavReport := models.FavoriteReport{
		ReportId: id,
		Login:    auth.GetUserMail(),
	}

	if err := DB.Model(&models.FavoriteReport{}).Create(&newFavReport).Error; err != nil {
		log.Error(fmt.Sprintf("error creating favorite report: %v", err))
		return err
	}

	return nil
}

func RemoveFavoriteReportById(id int) error {
	login := auth.ReturnDomainUser()

	var report models.Report
	if err := DB.Model(&models.Report{}).First(&report, "\"Id\" = ?", id).Error; err != nil {
		if err == gorm.ErrRecordNotFound {
			msg := fmt.Sprintf("report with this Id not found: %d", id)
			log.Error(msg)
			return fmt.Errorf(msg)
		} else {
			log.Error(fmt.Sprintf(errRunQuery, err))
			return err
		}
	}

	var favoriteReport models.FavoriteReport
	if err := DB.Model(&models.FavoriteReport{}).Where("\"Login\" = ? AND \"ReportId\" = ?", login, id).First(&favoriteReport).Error; err == nil {
		msg := "this report NOT in favorite"
		log.Error(msg)
		return fmt.Errorf(msg)
	}

	if err := DB.Model(&models.FavoriteReport{}).Where("\"ReportId\" = ?", id).Delete(&models.FavoriteReport{}).Error; err != nil {
		msg := fmt.Sprintf("failed to delete favorite report: %v", err)
		log.Error(msg)
		return fmt.Errorf(msg)
	}

	return nil
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
	var report models.Report
	err := DB.Where("\"Id\" = ?", uRep.Id).Select("*").Find(&report).Error
	if err != nil {
		msg := fmt.Sprintf("report with this id %d does not exist", uRep.Id)
		log.Error(msg)
		return fmt.Errorf(msg)
	}

	report.Owner = strings.ToLower(strings.TrimSpace(report.Owner))
	report.OperationName = strings.ToLower(strings.TrimSpace(report.OperationName))

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

	report.Text = uRep.Text
	report.URL = uRep.URL
	report.OperationName = uRep.OperationName
	report.UpdatedAt = time.Now()
	report.UpdatedBy = auth.ReturnDomainUser()

	if err := DB.Model(&report).Updates(map[string]interface{}{
		"Text":          report.Text,
		"URL":           report.URL,
		"OperationName": report.OperationName,
		"UpdatedAt":     report.UpdatedAt,
		"UpdatedBy":     report.UpdatedBy,
	}).Error; err != nil {
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

	var duplicateByURL models.Report
	if err := DB.Where("\"URL\" = ?", report.URL).First(&duplicateByURL).Error; err == nil {
		msg := fmt.Sprintf("report with this URL already exists. Report name: %s", duplicateByURL.Text)
		log.Error(msg)
		return nil, fmt.Errorf(msg)
	}

	var unknownCat models.Category
	if err := DB.Where("\"Id\" = ?", report.ParentId).First(&unknownCat).Error; err != nil {
		msg := fmt.Sprintf("category with this id %d does not exist", report.ParentId)
		log.Error(msg)
		return nil, fmt.Errorf(msg)
	}

	newReport := models.Report{
		Text:          report.Text,
		URL:           report.URL,
		Visible:       report.Visible,
		Alias:         report.Alias,
		Description:   report.Description,
		OperationName: report.OperationName,
		Owner:         report.Owner,
		CreatedBy:     auth.ReturnDomainUser(),
		CreatedAt:     time.Now(),
		UpdatedBy:     auth.ReturnDomainUser(),
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

	if len(report.Categories) == 0 {
		if err := DB.Delete(&report).Error; err != nil {
			msg := fmt.Sprintf("failed to delete report: %v", err)
			log.Error(msg)
			return fmt.Errorf(msg)
		}
	}

	return nil
}
