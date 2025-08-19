package database

import (
	"cmd/reporting-api/internal/models"
	"fmt"
	"time"

	log "krr-app-gitlab01.europe.mittalco.com/pait/modules/go/logging"
)

func ClearExpiredPrivateAliases() {
	now := time.Now()

	result := DB.Model(&models.Category{}).
		Where("\"PrivateAliasExpiresAt\" <= ?", now).
		Updates(map[string]interface{}{
			"PrivateAlias":          "",
			"PrivateAliasExpiresAt": nil,
		})
	if result.Error != nil {
		log.Error("failed to clear categories: " + result.Error.Error())
	} else {
		log.Info(fmt.Sprintf("cleared %d categories", result.RowsAffected))
	}

	result = DB.Model(&models.Report{}).
		Where("\"PrivateAliasExpiresAt\" <= ?", now).
		Updates(map[string]interface{}{
			"PrivateAlias":          "",
			"PrivateAliasExpiresAt": nil,
		})
	if result.Error != nil {
		log.Error("failed to clear reports: " + result.Error.Error())
	} else {
		log.Info(fmt.Sprintf("cleared %d reports", result.RowsAffected))
	}
}
