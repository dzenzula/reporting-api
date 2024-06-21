package middleware

import (
	"fmt"
	"net/http"
	"time"

	"cmd/reporting-api/internal/database"

	"github.com/gin-gonic/gin"
	"krr-app-gitlab01.europe.mittalco.com/pait/modules/go/logging"
)

func Logger() gin.HandlerFunc {
	return func(c *gin.Context) {
		startTime := time.Now()
		c.Next()
		duration := time.Since(startTime)
		logging.Info(fmt.Sprintf("%s %s %d %s", c.Request.Method, c.Request.URL.Path, c.Writer.Status(), duration))
	}
}

func DBConnectionChecker() gin.HandlerFunc {
	return func(c *gin.Context) {
		if c.FullPath() == "/reporting-api/api/Authorization/GetCurrentUserInfo" ||
			c.FullPath() == "/reporting-api/api/Authorization/LogInAuthorization" ||
			c.FullPath() == "/reporting-api/api/Authorization/LogOutAuthorization" {
			c.Next()
			return
		}

		err := database.IsDBConnected()
		if err != nil {
			logging.Error(fmt.Sprintf("database connection failed: %v", err))
			err = database.ConnectToPGDataBase()
			if err != nil {
				logging.Error(fmt.Sprintf("database connection failed: %v", err))
				c.JSON(http.StatusInternalServerError, gin.H{"error": fmt.Sprintf("database connection failed: %v", err)})
				return 
			}	
		}
		c.Next()
	}
}
