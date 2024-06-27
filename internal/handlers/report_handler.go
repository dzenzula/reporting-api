package handlers

import (
	"cmd/reporting-api/internal/config"
	"cmd/reporting-api/internal/models"
	"cmd/reporting-api/internal/services"
	"net/http"
	"strconv"

	"github.com/gin-gonic/gin"
	auth "krr-app-gitlab01.europe.mittalco.com/pait/modules/go/authorization"
)

// GetFavoriteReportsHandler godoc
// @Summary Get all favorite reports
// @Description Get a list of all favorite reports
// @Tags FavoriteReports
// @Produce json
// @Success 200 {array} models.FavoriteReport
// @Router /api/FavoriteReports/GetReports [get]
func GetFavoriteReportsHandler(c *gin.Context) {
	auth.Init(c)

	favReports := []models.Report{}
	res, err := services.GetAllFavoriteReports()
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
		return
	}

	favReports = append(favReports, res...)
	c.JSON(http.StatusOK, favReports)
}

// AddFavoriteReportHandler godoc
// @Summary Add a report to favorites
// @Description Add a report to favorites by report ID
// @Tags FavoriteReports
// @Produce json
// @Param id path int true "Report ID"
// @Success 200
// @Router /api/FavoriteReports/AddReport/{reportId} [post]
func AddFavoriteReportHandler(c *gin.Context) {
	auth.Init(c)
	id, err := strconv.Atoi(c.Param("reportId"))
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
		return
	}

	err = services.AddFavoriteReport(id)
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
		return
	}

	c.Status(http.StatusOK)
}

// RemoveFavoriteReportHandler godoc
// @Summary Remove a report from favorites
// @Description Remove a report from favorites by report ID
// @Tags FavoriteReports
// @Produce json
// @Param reportId path int true "Report ID"
// @Success 200
// @Router /api/FavoriteReports/DeleteReport/{reportId} [delete]
func RemoveFavoriteReportHandler(c *gin.Context) {
	id, err := strconv.Atoi(c.Param("reportId"))
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
		return
	}

	err = services.RemoveFavoriteReportById(id)
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
		return
	}

	c.Status(http.StatusOK)
}

// GetReportsHandler godoc
// @Summary Get all reports
// @Description Get a list of all reports
// @Tags Reports
// @Produce json
// @Success 200 {array} models.Report
// @Router /api/Reports [get]
func GetReportsHandler(c *gin.Context) {
	auth.Init(c)
	reports, err := services.GetAllReports()
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
		return
	}
	c.JSON(http.StatusOK, reports)
}

// UpdateReportHandler godoc
// @Summary Update a report
// @Description Update an existing report
// @Tags Reports
// @Accept json
// @Produce json
// @Param report body models.UpdateReport true "Report to update"
// @Success 200 {string} string "ok"
// @Router /api/Reports [put]
func UpdateReportHandler(c *gin.Context) {
	permissions := []string{config.GlobalConfig.Permissions.AdminAccess}
	if !checkPermissions(c, permissions) {
		return
	}

	var data models.UpdateReport
	if err := c.ShouldBindJSON(&data); err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
		return
	}

	if err := services.UpdateReport(data); err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
		return
	}

	c.Status(http.StatusOK)
}

// CreateReportHandler godoc
// @Summary Create a new report
// @Description Create a new report
// @Tags Reports
// @Accept json
// @Produce json
// @Param report body models.CreateReport true "Report to create"
// @Success 200 {integer} int "Created report ID"
// @Router /api/Reports [post]
func CreateReportHandler(c *gin.Context) {
	permissions := []string{config.GlobalConfig.Permissions.AdminAccess}
	if !checkPermissions(c, permissions) {
		return
	}

	var data models.CreateReport
	if err := c.ShouldBindJSON(&data); err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
		return
	}

	reportId, err := services.CreateReport(data)
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
		return
	}

	var buf [20]byte
	n := strconv.AppendInt(buf[:0], int64(*reportId), 10)

	c.Status(http.StatusOK)
	c.Writer.Write([]byte(n))
}

// UpdateCategoryReportsHandler godoc
// @Summary Update category reports
// @Description Update the category of a report
// @Tags Reports
// @Accept json
// @Produce json
// @Param data body models.UpdateCategoryParent true "Category data"
// @Success 200 {string} string "ok"
// @Router /api/Reports/UpdateCategoryReports [put]
func UpdateCategoryReportsHandler(c *gin.Context) {
	permissions := []string{config.GlobalConfig.Permissions.AdminAccess}
	if !checkPermissions(c, permissions) {
		return
	}

	var data models.UpdateCategoryParent
	if err := c.ShouldBindJSON(&data); err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
		return
	}

	if err := services.UpdateCategoryReport(data); err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
		return
	}

	c.Status(http.StatusOK)
}

// AddReportRelationHandler godoc
// @Summary Add report relation
// @Description Add a report to a category
// @Tags Reports
// @Accept json
// @Produce json
// @Param reportId path int true "Report ID"
// @Param categoryId body int true "Category ID"
// @Success 200 {string} string "ok"
// @Router /api/Reports/AddReportRelation/{reportId} [post]
func AddReportRelationHandler(c *gin.Context) {
	permissions := []string{config.GlobalConfig.Permissions.AdminAccess}
	if !checkPermissions(c, permissions) {
		return
	}

	reportId, err := strconv.Atoi(c.Param("reportId"))
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
		return
	}

	var categoryId int
	if err := c.ShouldBind(&categoryId); err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
		return
	}

	if err := services.AddReportRelation(reportId, categoryId); err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
		return
	}

	c.Status(http.StatusOK)
}

// RemoveReportHandler godoc
// @Summary Remove a report
// @Description Remove a report from a category
// @Tags Reports
// @Accept json
// @Produce json
// @Param reportId path int true "Report ID"
// @Param categoryId body int true "Category ID"
// @Success 200 {string} string "ok"
// @Router /api/Reports/{reportId} [delete]
func RemoveReportHandler(c *gin.Context) {
	permissions := []string{config.GlobalConfig.Permissions.AdminAccess}
	if !checkPermissions(c, permissions) {
		return
	}

	reportId, err := strconv.Atoi(c.Param("reportId"))
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
		return
	}

	var categoryId int
	if err := c.ShouldBind(&categoryId); err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
		return
	}

	if err := services.RemoveReportById(reportId, categoryId); err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
		return
	}

	c.Status(http.StatusOK)
}
