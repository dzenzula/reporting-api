package services

import (
	"cmd/reporting-api/internal/database"
	"cmd/reporting-api/internal/models"
)

func GetAllFavoriteReports() ([]models.FavoriteReportItem, error) {
	return database.FetchAllFavoriteReports()
}

func AddFavoriteReport(dto models.FavoriteReportDTO) error {
	return database.AddFavoriteReport(dto)
}

func GetLastVisitedReport(quantity int) ([]models.VisitedReport, error) {
	return database.GetLastVisitedReport(quantity)
}

func AddVisitedReport(dto models.TrackVisitDTO, ip string) error {
	return database.AddVisitedReport(dto, ip)
}

func RemoveFavoriteReport(dto models.FavoriteReportDTO) error {
	return database.RemoveFavoriteReport(dto)
}

func GetReportsForAdmin() ([]models.Report, error) {
	return database.FetchAllReportsForAdmin()
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
