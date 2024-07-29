package routes

import (
	c "cmd/reporting-api/internal/config"
	"cmd/reporting-api/internal/handlers"
	"cmd/reporting-api/internal/middleware"
	"net/http"

	"cmd/reporting-api/docs"

	"github.com/gin-contrib/sessions"
	"github.com/gin-contrib/sessions/cookie"
	"github.com/gin-gonic/gin"
	swaggerFiles "github.com/swaggo/files"
	ginSwagger "github.com/swaggo/gin-swagger"
	auth "krr-app-gitlab01.europe.mittalco.com/pait/modules/go/authorization"
)

func NewRouter() *gin.Engine {
	gin.SetMode(c.GlobalConfig.GinMode)
	docs.SwaggerInfo.Title = "Swagger Reporting API"
	docs.SwaggerInfo.Description = "This is a sample server Reporting API."
	docs.SwaggerInfo.Schemes = []string{"http", "https"}

	r := gin.New()

	r.Use(gin.Recovery())
	r.Use(middleware.Logger())
	r.Use(middleware.DBConnectionChecker())

	store := cookie.NewStore([]byte("secret"))
	store.Options(sessions.Options{
		Path:     "/reporting-api/api",
		HttpOnly: true,
		SameSite: http.SameSiteNoneMode,
		Secure:   true,
	})
	r.Use(sessions.Sessions("mysession", store))

	r.GET("/swagger/*any", ginSwagger.WrapHandler(swaggerFiles.Handler))
	authGroup := r.Group("/api/Authorization")
	{
		authGroup.GET("/GetCurrentUserInfo", auth.GetCurrentUserInfo)
		authGroup.POST("/LogInAuthorization", auth.LogInAuthorization)
		authGroup.POST("/LogOutAuthorization", auth.LogOutAuthorization)
	}

	catGroup := r.Group("/api/Categories")
	catGroup.GET("", handlers.GetCategoriesHandler)
	catGroup.Use(auth.AuthRequired)
	{
		catGroup.GET("/GetCategoriesForAdmin", handlers.GetCategoriesForAdminHandler)
		catGroup.PUT("", handlers.UpdateCategoryHandler)
		catGroup.POST("", handlers.CreateCategoryHandler)
		catGroup.PUT("/UpdateCategoryParent", handlers.ChangeCategoryParentHandler)
		catGroup.DELETE("/:categoryId", handlers.RemoveCategoryHandler)
	}

	favRepGroup := r.Group("/api/FavoriteReports")
	favRepGroup.Use(auth.AuthRequired)
	{
		favRepGroup.GET("/GetReports", handlers.GetFavoriteReportsHandler)
		favRepGroup.POST("/AddReport/:reportId", handlers.AddFavoriteReportHandler)
		favRepGroup.DELETE("/DeleteReport/:reportId", handlers.RemoveFavoriteReportHandler)
	}

	repGroup := r.Group("/api/Reports")
	repGroup.GET("", handlers.GetReportsHandler)
	repGroup.Use(auth.AuthRequired)
	{
		repGroup.PUT("", handlers.UpdateReportHandler)
		repGroup.POST("", handlers.CreateReportHandler)
		repGroup.PUT("/UpdateCategoryReports", handlers.UpdateCategoryReportsHandler)
		repGroup.POST("/AddReportRelation/:reportId", handlers.AddReportRelationHandler)
		repGroup.DELETE("/:reportId", handlers.RemoveReportHandler)
	}

	return r
}
