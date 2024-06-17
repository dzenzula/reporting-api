package handlers

import (
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
	favReports, err := services.GetAllFavoriteReports()
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
		return
	}
	c.JSON(http.StatusOK, favReports)
}

// AddFavoriteReportHandler godoc
// @Summary Add a report to favorites
// @Description Add a report to favorites by report ID
// @Tags FavoriteReports
// @Produce json
// @Param id query int true "Report ID"
// @Success 200
// @Router /api/FavoriteReports/AddReport [post]
func AddFavoriteReportHandler(c *gin.Context) {
	auth.Init(c)
	id, err := strconv.Atoi(c.Query("id"))
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
// @Param id query int true "Report ID"
// @Success 200
// @Router /api/FavoriteReports/DeleteReport [delete]
func RemoveFavoriteReportHandler(c *gin.Context) {
	id, err := strconv.Atoi(c.Query("id"))
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

func GetReportsHandler(c *gin.Context) {
	/*reports, err := services.GetAllReports()
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
		return
	}
	c.JSON(http.StatusOK, reports)*/
}

func UpdateReportHandler(c *gin.Context) {

}

func CreateReportHandler(c *gin.Context) {

}

func UpdateCategoryReportsHandler(c *gin.Context) {

}

func AddReportRelationHandler(c *gin.Context) {

}

func RemoveReportHandler(c *gin.Context) {

}
