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
	if err := DB.Table("dbo.Categories").Find(&categories).Error; err != nil {
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

	err := DB.Table("Categories c").
		Select("DISTINCT c.*").
		Joins("JOIN CategoryReports cr ON cr.CategoriesId = c.Id").
		Joins("JOIN Reports r ON cr.ReportsId = r.Id AND r.Operation_name = ?", "public").
		Where("c.Visible = ?", true).
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

func CreateCategory(newCategory models.InsertCategory) error {
	category := models.Category{
		Text:        newCategory.Text,
		Description: newCategory.Description,
		ParentId:    newCategory.ParentId,
		Visible:     newCategory.Visible,
		CreatedBy:   authorization.ReturnDomainUser(),
		CreatedAt:   time.Now(),
	}

	if err := DB.Table("Categories").Create(&category).Error; err != nil {
		log.Error(fmt.Sprintf("failed to insert category: %v", err))
		return err
	}
	return nil
}

func UpdateCategory(updatedCategory models.UpdateCategory) error {
	category := models.Category{
		Id:          updatedCategory.Id,
		Text:        updatedCategory.Text,
		Description: updatedCategory.Description,
		Visible:     updatedCategory.Visible,
		CreatedBy:   authorization.ReturnDomainUser(),
		CreatedAt:   time.Now(),
	}

	if err := DB.Table("Categories").Save(&category).Error; err != nil {
		log.Error(fmt.Sprintf("failed to update category: %v", err))
		return err
	}
	return nil
}

func ChangeCategoryParent(updateParent models.UpdateCategoryParent) error {
	if err := DB.Table("Categories").Where("Id = ?", updateParent.Id).Update("ParentId", updateParent.ToCat).Update("UpdatedBy", authorization.ReturnDomainUser()).Error; err != nil {
		log.Error(fmt.Sprintf("failed to update category parent: %v", err))
		return err
	}
	return nil
}

func RemoveCategoryById(categoryId int) error {
	if err := DB.Table("Categories").Where("Id = ?", categoryId).Delete(&models.Category{}).Error; err != nil {
		log.Error(fmt.Sprintf("failed to delete category: %v", err))
		return err
	}
	return nil
}
