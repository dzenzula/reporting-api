package middleware

import (
	"fmt"
	"time"

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
