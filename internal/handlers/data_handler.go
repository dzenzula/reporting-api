package handlers

import (
	"cmd/reporting-api/internal/services"
	"net/http"

	"github.com/gin-gonic/gin"
)

// GetAllVisibleFlatHandler godoc
// @Summary Get all visible categories and reports
// @Description Returns all visible categories and reports (empty path or two aliases)
// @Tags data
// @Accept json
// @Produce json
// @Success 200 {object} models.PathDataResponse
// @Failure 500 {object} map[string]string
// @Router /api/data [get]
// @Router /api/data/{alias}/{subalias} [get]
func GetAllVisibleFlatHandler(c *gin.Context) {
	response, err := services.GetAllVisibleFlat()
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
		return
	}
	c.JSON(http.StatusOK, response)
}

// SingleAliasHandler godoc
// @Summary Get category or report by single alias
// @Description Returns categories and reports for a single alias (public or private)
// @Tags data
// @Accept json
// @Produce json
// @Param alias path string true "Alias (category or private alias)"
// @Success 200 {object} models.PathDataResponse
// @Failure 404 {object} map[string]string
// @Failure 500 {object} map[string]string
// @Router /api/data/{alias} [get]
func SingleAliasHandler(c *gin.Context) {
	alias := c.Param("alias")

	response, err := services.HandleSingleAlias(alias)
	if err != nil {
		c.JSON(http.StatusNotFound, gin.H{"error": err.Error()})
		return
	}
	c.JSON(http.StatusOK, response)
}
