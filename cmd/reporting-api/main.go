package main

import (
	routes "cmd/reporting-api/api/v1"
	c "cmd/reporting-api/internal/config"
	"cmd/reporting-api/internal/database"

	log "krr-app-gitlab01.europe.mittalco.com/pait/modules/go/logging"
)

// @title Reporting API
// @version 1.0
// @description This is a sample server for reporting API.
// @BasePath /reporting-api/

func main() {
	err := c.Load()
	if err != nil {
		log.Error(err.Error())
		return
	}
	log.LogInit(c.GlobalConfig.LogLevel)
	err = database.ConnectToPGDataBase()
	if err != nil {
		log.Error(err.Error())
		return
	}
	defer database.Close()

	r := routes.NewRouter()
	//r.GET("/swagger/*any", ginSwagger.WrapHandler(swaggerFiles.Handler))
	r.Run(c.GlobalConfig.ServerAddress)
}
