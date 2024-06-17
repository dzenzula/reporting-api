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
