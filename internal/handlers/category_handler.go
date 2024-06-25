package handlers

import (
	"cmd/reporting-api/internal/config"
	"cmd/reporting-api/internal/models"
	"cmd/reporting-api/internal/services"
	"net/http"
	"strconv"

	"github.com/gin-gonic/gin"
	"krr-app-gitlab01.europe.mittalco.com/pait/modules/go/authorization"
)

// @Summary Get Categories for Admin
// @Description Получение списка категорий для администратора
// @Tags Categories
// @Accept  json
// @Produce  json
// @Success 200 {array} models.GetCategory
// @Router /api/Categories/GetCategoriesForAdmin [get]
func GetCategoriesForAdminHandler(c *gin.Context) {
	permissions := []string{config.GlobalConfig.Permissions.AdminAccess}
	if !checkPermissions(c, permissions) {
		return
	}

	categories, err := services.GetCategoriesForAdmin()
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
		return
	}
	c.JSON(http.StatusOK, categories)
}

// @Summary Get Categories
// @Description Получение списка категорий
// @Tags Categories
// @Accept  json
// @Produce  json
// @Success 200 {array} models.GetCategory
// @Router /api/Categories [get]
func GetCategoriesHandler(c *gin.Context) {
	categories, err := services.GetCategories()
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
		return
	}
	c.JSON(http.StatusOK, categories)
}

// @Summary Insert Category
// @Description Создать категорию
// @Tags Categories
// @Accept  json
// @Produce  json
// @Param data body models.InsertCategory true "Новая категория"
// @Success 200 {integer} int "Created Category Id"
// @Router /api/Categories [post]
func CreateCategoryHandler(c *gin.Context) {
	var data models.InsertCategory
	permissions := []string{config.GlobalConfig.Permissions.AdminAccess}
	if !checkPermissions(c, permissions) {
		return
	}

	if err := c.ShouldBindJSON(&data); err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
		return
	}

	categoryId, err := services.CreateCategory(data)
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
		return
	}
	var buf [20]byte
	n := strconv.AppendInt(buf[:0], int64(*categoryId), 10)

	c.Status(http.StatusOK)
	c.Writer.Write([]byte(n))
}

// @Summary Update Category
// @Description Обновить категорию
// @Tags Categories
// @Accept  json
// @Produce  json
// @Param data body models.UpdateCategory true "Обновленная категория"
// @Success 200
// @Router /api/Categories [put]
func UpdateCategoryHandler(c *gin.Context) {
	var data models.UpdateCategory
	permissions := []string{config.GlobalConfig.Permissions.AdminAccess}
	if !checkPermissions(c, permissions) {
		return
	}

	if err := c.ShouldBindJSON(&data); err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
		return
	}

	err := services.UpdateCategory(data)
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
		return
	}
	c.Status(http.StatusOK)
}

// @Summary Update Parent Category
// @Description Обновить родительскую категорию
// @Tags Categories
// @Accept  json
// @Produce  json
// @Param data body models.UpdateCategoryParent true "Обновленный родительский айди"
// @Success 200
// @Router /api/Categories/UpdateCategoryParent [put]
func ChangeCategoryParentHandler(c *gin.Context) {
	var data models.UpdateCategoryParent
	permissions := []string{config.GlobalConfig.Permissions.AdminAccess}
	if !checkPermissions(c, permissions) {
		return
	}

	if err := c.ShouldBindJSON(&data); err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
		return
	}

	err := services.ChangeCategoryParent(data)
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
		return
	}
	c.Status(http.StatusOK)
}

// @Summary Delete Category
// @Description Удалить категорию
// @Tags Categories
// @Accept  json
// @Produce  json
// @Param categoryId path int true "Id категории"
// @Success 200
// @Router /api/Categories/{categoryId} [delete]
func RemoveCategoryHandler(c *gin.Context) {
	permissions := []string{config.GlobalConfig.Permissions.AdminAccess}
	if !checkPermissions(c, permissions) {
		return
	}

	id, err := strconv.Atoi(c.Param("categoryId"))
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
		return
	}

	err = services.RemoveCategoryById(id)
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
		return
	}
	c.Status(http.StatusOK)
}

func checkPermissions(c *gin.Context, permissions []string) bool {
	authorization.Init(c)
	checkPermissions := authorization.CheckAnyPermission(permissions)
	if checkPermissions != authorization.Ok {
		c.JSON(http.StatusBadRequest, gin.H{"error": string(checkPermissions)})
		return false
	}
	return true
}
