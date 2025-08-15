package services

import (
	"cmd/reporting-api/internal/database"
	"cmd/reporting-api/internal/models"
	"fmt"
)

func GetVisibleDataFlat() (*models.PathDataResponse, error) {
	response, err := database.FetchVisibleDataFlat()
	if err != nil {
		return nil, fmt.Errorf("failed to get all visible data: %v", err)
	}
	return response, nil
}

func HandleSingleAlias(alias string) (*models.PathDataResponse, error) {
	response, err := database.HandleSingleAlias(alias)
	if err != nil {
		return nil, fmt.Errorf("alias not found: %s", alias)
	}
	return response, nil
}
