package models

type Config struct {
	Permissions   Permissions `yaml:"permissions"`
	PgConnection  PGSQL       `yaml:"postgres"`
	LogLevel      string      `yaml:"log_level"`
	GinMode       string      `yaml:"gin_mode"`
	ServerAddress string      `yaml:"server_address"`
}

type Permissions struct {
	AdminAccess string `yaml:"admin_access"`
	FavReports  string `yaml:"fav_reports"`
}

type PGSQL struct {
	Host     string `yaml:"host"`
	Port     int    `yaml:"port"`
	UserName string `yaml:"username"`
	Password string `yaml:"password"`
	DbName   string `yaml:"dbname"`
	SSLMode  string `yaml:"sslmode"`
}
