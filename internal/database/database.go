package database

import (
	"fmt"

	c "cmd/reporting-api/internal/config"

	log "krr-app-gitlab01.europe.mittalco.com/pait/modules/go/logging"

	"gorm.io/driver/postgres"
	"gorm.io/gorm"
	"gorm.io/gorm/logger"
)

var DB *gorm.DB

func ConnectToPGDataBase() error {
	var err error
	connString := fmt.Sprintf(
		"host=%s port=%d user=%s password=%s dbname=%s sslmode=%s",
		c.GlobalConfig.PgConnection.Host,
		c.GlobalConfig.PgConnection.Port,
		c.GlobalConfig.PgConnection.UserName,
		c.GlobalConfig.PgConnection.Password,
		c.GlobalConfig.PgConnection.DbName,
		c.GlobalConfig.PgConnection.SSLMode,
	)

	DB, err = gorm.Open(postgres.Open(connString), &gorm.Config{})
	if err != nil {
		return err
	}

	DB.Logger.LogMode(logger.Info)

	return nil
}

func IsDBConnected() error {
	sqlDB, err := DB.DB()
	if err != nil {
		return err
	}
	if err := sqlDB.Ping(); err != nil {
		return err
	}
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
