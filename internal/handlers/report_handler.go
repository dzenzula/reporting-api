package handlers

import (
	"cmd/reporting-api/internal/config"
	"cmd/reporting-api/internal/models"
	"cmd/reporting-api/internal/services"
	"net/http"
	"strconv"
	"strings"

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

	favReports := []models.FavoriteReportItem{}
	res, err := services.GetAllFavoriteReports()
	if err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"error": err.Error()})
		return
	}

	favReports = append(favReports, res...)
	c.JSON(http.StatusOK, favReports)
}

// AddFavoriteReportHandler godoc
// @Summary Add a report to favorites
// @Description Add a report to favorites by report ID and category ID
// @Tags FavoriteReports
// @Accept json
// @Produce json
// @Param request body models.FavoriteReportDTO true "Favorite report request"
// @Success 200
// @Failure 400 {object} map[string]string
// @Router /api/FavoriteReports/AddReport [post]
func AddFavoriteReportHandler(c *gin.Context) {
	auth.Init(c)

	var dto models.FavoriteReportDTO

	if err := c.ShouldBindJSON(&dto); err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"error": err.Error()})
		return
	}

	err := services.AddFavoriteReport(dto)
	if err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"error": err.Error()})
		return
	}

	c.Status(http.StatusOK)
}

// RemoveFavoriteReportHandler godoc
// @Summary Remove a report from favorites
// @Description Remove a report from favorites by report ID and category ID
// @Tags FavoriteReports
// @Accept json
// @Produce json
// @Param request body models.FavoriteReportDTO true "Favorite report request"
// @Success 200
// @Failure 400 {object} map[string]string
// @Router /api/FavoriteReports/DeleteReport [delete]
func RemoveFavoriteReportHandler(c *gin.Context) {
	auth.Init(c)

	var dto models.FavoriteReportDTO

	if err := c.ShouldBindJSON(&dto); err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"error": err.Error()})
		return
	}

	err := services.RemoveFavoriteReport(dto)
	if err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"error": err.Error()})
		return
	}

	c.Status(http.StatusOK)
}

// GetLastVisitedHandler godoc
// @Summary Get last visited reports
// @Description Get last visited reports
// @Tags VisitedReports
// @Produce json
// @Param quantity query int false "Quantity of reports to return" default(20)
// @Success 200 {array} models.VisitedReport
// @Router /api/TrackVisit/GetLastVisited [get]
func GetLastVisitedHandler(c *gin.Context) {
	auth.Init(c)

	quantityStr := c.DefaultQuery("quantity", "20")

	quantity, err := strconv.Atoi(quantityStr)
	if err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"error": "Invalid quantity"})
		return
	}

	reports := []models.VisitedReport{}
	res, err := services.GetLastVisitedReport(quantity)
	if err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"error": err.Error()})
		return
	}

	reports = append(reports, res...)
	c.JSON(http.StatusOK, reports)
}

// AddVisitHandler godoc
// @Summary Add visited report
// @Description Track visited reports
// @Tags VisitHistory
// @Accept json
// @Produce json
// @Param input body models.TrackVisitDTO true "Report and Category IDs"
// @Success 200
// @Failure 400 {object} map[string]string
// @Router /api/TrackVisit [post]
func AddVisitHandler(c *gin.Context) {
	auth.Init(c)

	var dto models.TrackVisitDTO
	if err := c.ShouldBindJSON(&dto); err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"error": "Invalid input: " + err.Error()})
		return
	}

	clientAddress := strings.Split(c.GetHeader("X-Forwarded-For"), ",")[0]
	ip := strings.Split(clientAddress, ":")[0]

	err := services.AddVisitedReport(dto, ip)
	if err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"error": err.Error()})
		return
	}

	c.Status(http.StatusOK)
}

// @Summary Get Reports for Admin
// @Description Получение списка отчетов для администратора
// @Tags Reports
// @Accept  json
// @Produce  json
// @Success 200 {array} models.Report
// @Router /api/Reports/GetReportsForAdmin [get]
func GetReportsForAdminHandler(c *gin.Context) {
	permissions := []string{config.GlobalConfig.Permissions.AdminAccess}
	if !checkPermissions(c, permissions) {
		return
	}

	categories, err := services.GetReportsForAdmin()
	if err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"error": err.Error()})
		return
	}
	c.JSON(http.StatusOK, categories)
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
		c.JSON(http.StatusBadRequest, gin.H{"error": err.Error()})
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
		c.JSON(http.StatusBadRequest, gin.H{"error": err.Error()})
		return
	}

	if err := services.UpdateReport(data); err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"error": err.Error()})
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
		c.JSON(http.StatusBadRequest, gin.H{"error": err.Error()})
		return
	}

	reportId, err := services.CreateReport(data)
	if err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"error": err.Error()})
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
		c.JSON(http.StatusBadRequest, gin.H{"error": err.Error()})
		return
	}

	if err := services.UpdateCategoryReport(data); err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"error": err.Error()})
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
		c.JSON(http.StatusBadRequest, gin.H{"error": err.Error()})
		return
	}

	var categoryId int
	if err := c.ShouldBind(&categoryId); err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"error": err.Error()})
		return
	}

	if err := services.AddReportRelation(reportId, categoryId); err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"error": err.Error()})
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
		c.JSON(http.StatusBadRequest, gin.H{"error": err.Error()})
		return
	}

	var categoryId int
	if err := c.ShouldBind(&categoryId); err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"error": err.Error()})
		return
	}

	if err := services.RemoveReportById(reportId, categoryId); err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"error": err.Error()})
		return
	}

	c.Status(http.StatusOK)
}
