package database

import (
	"cmd/reporting-api/internal/models"
	"fmt"

	"gorm.io/gorm"
	auth "krr-app-gitlab01.europe.mittalco.com/pait/modules/go/authorization"
	log "krr-app-gitlab01.europe.mittalco.com/pait/modules/go/logging"
)

const (
	errRunQuery string = "error running query: %s"
)

func FetchAllFavoriteReports() ([]models.Report, error) {
	var favoriteReports []models.Report
	err := DB.Table("FavoriteReports").
		Select("FavoriteReports.*, Reports.*").
		Joins("JOIN Reports ON Reports.Id = FavoriteReports.ReportId AND Reports.Owner = FavoriteReports.Login").
		Scan(&favoriteReports).Error
	if err != nil {
		log.Error(fmt.Sprintf(errRunQuery, err))
		return nil, err
	}

	for i := range favoriteReports {
		favoriteReports[i].Type = "folder"
		favoriteReports[i].Data.URL = favoriteReports[i].URL
	}

	return favoriteReports, nil
}

func AddFavoriteReport(id int) error {
	var report models.Report
	if err := DB.Table("Reports").First(&report, "Id = ?", id).Error; err != nil {
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
	if err := DB.Table("FavoriteReports").First(&favoriteReport, "ReportId = ?", id).Error; err == nil {
		msg := "this report already in favorite"
		log.Error(msg)
		return fmt.Errorf(msg)
	}

	newFavReport := models.FavoriteReport{
		ReportId: id,
		Login:    auth.ReturnDomainUser(),
	}

	if err := DB.Table("FavoriteReports").Create(&newFavReport).Error; err != nil {
		log.Error(err.Error())
		return err
	}

	return nil
}

func RemoveFavoriteReportById(id int) error {
	login := auth.ReturnDomainUser()

	var report models.Report
	if err := DB.First(&report, "ReportId = ?", id).Error; err != nil {
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
	if err := DB.Where("Login = ? AND ReportId = ?", login, id).First(&favoriteReport).Error; err == nil {
		msg := "this report NOT in favorite"
		log.Error(msg)
		return fmt.Errorf(msg)
	}

	if err := DB.Table("FavoriteReports").Where("ReportId = ?", id).Delete(&models.FavoriteReport{}).Error; err != nil {
		msg := fmt.Sprintf("failed to delete favorite report: %v", err)
		log.Error(msg)
		return fmt.Errorf(msg)
	}

	return nil
}
