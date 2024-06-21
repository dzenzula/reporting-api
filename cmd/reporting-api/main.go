package main

import (
	routes "cmd/reporting-api/api/v1"
	c "cmd/reporting-api/internal/config"
	"cmd/reporting-api/internal/database"
	"fmt"

	"github.com/kardianos/service"
	log "krr-app-gitlab01.europe.mittalco.com/pait/modules/go/logging"
)

// @title Reporting API
// @version 1.0
// @description This is a sample server for reporting API.
// @BasePath /reporting-api

type program struct{}

func (p *program) Start(s service.Service) error {
	go p.run()
	return nil
}

func (p *program) Stop(s service.Service) error {
	return nil
}

func (p *program) run() {
	startApi()
}

func startApi() {
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
	r.Run(c.GlobalConfig.ServerAddress)
}

func main() {
	svcConfig := &service.Config{
		Name:        "ReportingApi",
		DisplayName: "Reporting Api Service",
		Description: "",
	}

	prg := &program{}
	s, err := service.New(prg, svcConfig)
	if err != nil {
		log.Error(fmt.Sprintf("Error creating service: %s", err))
		return
	}

	if err = s.Run(); err != nil {
		log.Error(fmt.Sprintf("Error starting service: %s", err))
		return
	}
}
