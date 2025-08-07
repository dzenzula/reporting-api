package database

import (
	"cmd/reporting-api/internal/models"
	"fmt"
	"time"

	"krr-app-gitlab01.europe.mittalco.com/pait/modules/go/authorization"
	log "krr-app-gitlab01.europe.mittalco.com/pait/modules/go/logging"
)

func FetchAllCategoriesForAdmin() ([]models.GetCategory, error) {
	var categories []models.GetCategory
	if err := DB.Find(&categories).Error; err != nil {
		log.Error(fmt.Sprintf("failed to query categories: %v", err))
		return nil, err
	}

	for i := range categories {
		categories[i].Type = "folder"
	}

	return categories, nil
}

func FetchVisiblePublicCategories() ([]models.GetCategory, error) {
	var categories []models.GetCategory

	err := DB.Table("\"sys-reporting\".\"Categories\" AS c").
		Select("DISTINCT c.*").
		Joins("JOIN \"sys-reporting\".\"CategoryReports\" cr ON cr.\"CategoriesId\" = c.\"Id\"").
		Joins("JOIN \"sys-reporting\".\"Reports\" r ON cr.\"ReportsId\" = r.\"Id\" AND r.\"OperationName\" = ?", "public").
		Where("c.\"Visible\" = ?", true).
		Find(&categories).Error
	if err != nil {
		log.Error(fmt.Sprintf("failed to query categories: %v", err))
		return nil, err
	}

	for i := range categories {
		categories[i].Type = "folder"
	}

	return categories, nil
}

func CreateCategory(newCategory models.InsertCategory) (*int, error) {
	var duplicateByAlias models.Category
	if err := DB.Where("\"Alias\" = ?", newCategory.Alias).First(&duplicateByAlias).Error; err == nil {
		msg := fmt.Sprintf("category with this alias already exists. Category name: %s", duplicateByAlias.Text)
		log.Error(msg)
		return nil, fmt.Errorf(msg)
	}

	category := models.Category{
		Text:        newCategory.Text,
		Description: newCategory.Description,
		Alias:       newCategory.Alias,
		ParentId:    newCategory.ParentId,
		Visible:     newCategory.Visible,
		CreatedBy:   authorization.ReturnDomainUser(),
		CreatedAt:   time.Now(),
	}

	if err := DB.Create(&category).Error; err != nil {
		log.Error(fmt.Sprintf("failed to insert category: %v", err))
		return nil, err
	}
	return category.Id, nil
}

func UpdateCategory(updatedCategory models.UpdateCategory) error {
	updateData := map[string]interface{}{
		"Text":        updatedCategory.Text,
		"Description": updatedCategory.Description,
		"Visible":     updatedCategory.Visible,
		"UpdatedBy":   authorization.ReturnDomainUser(),
		"UpdatedAt":   time.Now(),
	}

	if err := DB.Model(&models.Category{}).
		Where("\"Id\" = ?", updatedCategory.Id).
		Updates(updateData).Error; err != nil {
		log.Error(fmt.Sprintf("failed to update category: %v", err))
		return err
	}
	return nil
}

func ChangeCategoryParent(updateParent models.UpdateCategoryParent) error {
	updateData := map[string]interface{}{
		"ParentId":  updateParent.ToCat,
		"UpdatedBy": authorization.ReturnDomainUser(),
		"UpdatedAt": time.Now(), // Обновляем время
	}

	if err := DB.Model(&models.Category{}).
		Where("\"Id\" = ?", updateParent.Id).
		Updates(updateData).Error; err != nil {
		log.Error(fmt.Sprintf("failed to update category parent: %v", err))
		return err
	}
	return nil
}

func RemoveCategoryById(categoryId int) error {
	if err := DB.Where("\"Id\" = ?", categoryId).Delete(&models.Category{}).Error; err != nil {
		log.Error(fmt.Sprintf("failed to delete category: %v", err))
		return err
	}
	return nil
}
