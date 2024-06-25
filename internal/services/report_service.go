package services

import (
	"cmd/reporting-api/internal/database"
	"cmd/reporting-api/internal/models"
)

func GetAllFavoriteReports() ([]models.Report, error) {
	return database.FetchAllFavoriteReports()
}

func AddFavoriteReport(id int) error {
	return database.AddFavoriteReport(id)
}

func RemoveFavoriteReportById(id int) error {
	return database.RemoveFavoriteReportById(id)
}

func GetAllReports() ([]models.Report, error) {
	return database.FetchAllReports()
}

func UpdateReport(uRep models.UpdateReport) error {
	return database.UpdateReport(uRep)
}

func UpdateCategoryReport(uCatRep models.UpdateCategoryParent) error {
	return database.UpdateCategoryReport(uCatRep)
}

func AddReportRelation(reportID, categoryID int) error {
	return database.AddReportRelation(reportID, categoryID)
}

func CreateReport(report models.CreateReport) (*int, error) {
	return database.CreateReport(report)
}

func RemoveReportById(reportID, categoryID int) error {
	return database.RemoveReportById(reportID, categoryID)
}
