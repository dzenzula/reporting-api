package database

import (
	"fmt"

	c "cmd/reporting-api/internal/config"

	log "krr-app-gitlab01.europe.mittalco.com/pait/modules/go/logging"

	"gorm.io/driver/sqlserver"
	"gorm.io/gorm"
	"gorm.io/gorm/logger"
)

var DB *gorm.DB

func ConnectToMSDataBase() error {
	var err error
	connString := fmt.Sprintf("server=%s;user=%s;password=%s;database=%s;schema=auth;encrypt=disable",
		c.GlobalConfig.MssqlConnection.Server,
		c.GlobalConfig.MssqlConnection.User,
		c.GlobalConfig.MssqlConnection.Password,
		c.GlobalConfig.MssqlConnection.Database,
	)

	DB, err = gorm.Open(sqlserver.Open(connString), &gorm.Config{})
	if err != nil {
		return err
	}

	DB.Logger.LogMode(logger.Info)

	return nil
}

func Close() {
	db, err := DB.DB()
	if err != nil {
		log.Error(err.Error())
	}

	err = db.Close()
	if err != nil {
		log.Error("Error closing the database: " + err.Error())
	}
}
