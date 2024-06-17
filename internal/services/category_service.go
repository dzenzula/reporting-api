package services

import (
	"cmd/reporting-api/internal/database"
	"cmd/reporting-api/internal/models"
)

func GetCategoriesForAdmin() ([]models.GetCategory, error) {
	return database.FetchAllCategoriesForAdmin()
}

func GetCategories() ([]models.GetCategory, error) {
	return database.FetchVisiblePublicCategories()
}

func UpdateCategory(updatedCategory models.UpdateCategory) error {
	return database.UpdateCategory(updatedCategory)
}

func CreateCategory(newCategory models.InsertCategory) error {
	return database.CreateCategory(newCategory)
}

func ChangeCategoryParent(updateParent models.UpdateCategoryParent) error {
	return database.ChangeCategoryParent(updateParent)
}

func RemoveCategoryById(categoryId int) error {
	return database.RemoveCategoryById(categoryId)
}
