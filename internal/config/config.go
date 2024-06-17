package config

import (
	"cmd/reporting-api/internal/models"
	"os"
	"path/filepath"

	"gopkg.in/yaml.v2"
)

const (
	localConfig   string = "c:\\Personal\\reporting-api\\internal\\config\\config.yml"
	releaseConfig string = "reporting-api.conf.yml"
)

var GlobalConfig models.Config

func Load() error {
	executable, err := os.Executable()
	if err != nil {
		return err
	}
	releaseConfig := filepath.Join(filepath.Dir(executable), releaseConfig)
	configFiles := []string{localConfig, releaseConfig}
	var configName string

	for _, configFile := range configFiles {
		if _, err := os.Stat(configFile); err == nil {
			configName = configFile
			break
		}
	}

	data, err := os.ReadFile(configName)
	if err != nil {
		return err
	}

	err = yaml.Unmarshal(data, &GlobalConfig)
	if err != nil {
		return err
	}

	return nil
}
