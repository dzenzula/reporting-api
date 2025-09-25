package database

import (
	"cmd/reporting-api/internal/models"
	"fmt"
	"regexp"
	"strings"
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
	if strings.TrimSpace(newCategory.Text) == "" {
		return nil, fmt.Errorf("Name cannot be empty")
	}

	if err := models.ValidateAlias(newCategory.Alias); err != nil {
		return nil, err
	}

	privateAliasTrimmed := strings.TrimSpace(newCategory.PrivateAlias)
	expiresAtTrimmed := strings.TrimSpace(newCategory.PrivateAliasExpiresAt)

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

		if privateAliasTrimmed == newCategory.Alias {
			return nil, fmt.Errorf("Private Alias cannot be the same as Alias of the new category")
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

	var duplicateByAlias models.Category
	if err := DB.Where("\"Alias\" = ?", newCategory.Alias).First(&duplicateByAlias).Error; err == nil {
		msg := fmt.Sprintf("Category with this alias already exists. Category name: %s", duplicateByAlias.Text)
		log.Error(msg)
		return nil, fmt.Errorf(msg)
	}

	var duplicateByPrivateAlias models.Category
	if err := DB.Where("\"PrivateAlias\" = ?", newCategory.Alias).First(&duplicateByPrivateAlias).Error; err == nil {
		msg := fmt.Sprintf("Alias already used as Private Alias in categories. Category name: %s", duplicateByPrivateAlias.Text)
		log.Error(msg)
		return nil, fmt.Errorf(msg)
	}

	var duplicateByPrivateAliasInReports models.Report
	if err := DB.Where("\"PrivateAlias\" = ?", newCategory.Alias).First(&duplicateByPrivateAliasInReports).Error; err == nil {
		msg := fmt.Sprintf("Alias already used as Private Alias in reports. Report name: %s", duplicateByPrivateAliasInReports.Text)
		log.Error(msg)
		return nil, fmt.Errorf(msg)
	}

	category := models.Category{
		Text:                  newCategory.Text,
		Description:           newCategory.Description,
		Alias:                 newCategory.Alias,
		ParentId:              newCategory.ParentId,
		PrivateAlias:          privateAliasTrimmed,
		PrivateAliasExpiresAt: parsedExpiresAt,
		Visible:               newCategory.Visible,
		CreatedBy:             authorization.ReturnDomainUser(),
		CreatedAt:             time.Now(),
	}

	if err := DB.Create(&category).Error; err != nil {
		log.Error(fmt.Sprintf("failed to insert category: %v", err))
		return nil, err
	}
	return category.Id, nil
}

func UpdateCategory(updatedCategory models.UpdateCategory) error {
	if strings.TrimSpace(updatedCategory.Text) == "" {
		return fmt.Errorf("Name cannot be empty")
	}

	privateAliasTrimmed := strings.TrimSpace(updatedCategory.PrivateAlias)
	expiresAtTrimmed := strings.TrimSpace(updatedCategory.PrivateAliasExpiresAt)

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

	if privateAliasTrimmed != "" {
		if expiresAtTrimmed == "" {
			return fmt.Errorf("Set expiration date for Private Alias")
		}
		if len(privateAliasTrimmed) != 8 {
			return fmt.Errorf("Private Alias must be 8 characters")
		}

		var validAlias = regexp.MustCompile(`^[a-zA-Z0-9]+$`)
		if !validAlias.MatchString(privateAliasTrimmed) {
			return fmt.Errorf("Private Alias must contain only letters and digits")
		}

		var currentCategory models.Category
		if err := DB.Where("\"Id\" = ?", updatedCategory.Id).First(&currentCategory).Error; err != nil {
			return fmt.Errorf("Category %s not found", updatedCategory.Text)
		}

		if privateAliasTrimmed == currentCategory.Alias {
			return fmt.Errorf("Private Alias cannot be the same as Alias of the category")
		}

		var duplicateCategory models.Category
		if err := DB.Where("(\"Alias\" = ? OR \"PrivateAlias\" = ?) AND \"Id\" != ?",
			privateAliasTrimmed, privateAliasTrimmed, updatedCategory.Id).
			First(&duplicateCategory).Error; err == nil {
			msg := fmt.Sprintf("Private Alias already exists in categories. Category name: %s", duplicateCategory.Text)
			log.Error(msg)
			return fmt.Errorf(msg)
		}

		var duplicateReport models.Report
		if err := DB.Where("\"Alias\" = ? OR \"PrivateAlias\" = ?", privateAliasTrimmed, privateAliasTrimmed).
			First(&duplicateReport).Error; err == nil {
			msg := fmt.Sprintf("Private Alias already exists in reports. Report name: %s", duplicateReport.Text)
			log.Error(msg)
			return fmt.Errorf(msg)
		}
	}

	updateData := map[string]interface{}{
		"Text":                  updatedCategory.Text,
		"Description":           updatedCategory.Description,
		"Visible":               updatedCategory.Visible,
		"PrivateAlias":          updatedCategory.PrivateAlias,
		"PrivateAliasExpiresAt": parsedExpiresAt,
		"UpdatedBy":             authorization.ReturnDomainUser(),
		"UpdatedAt":             time.Now(),
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
