package models

type Config struct {
	Permissions     Permissions `yaml:"permissions"`
	MssqlConnection MSSQL       `yaml:"mssql"`
	PgConnection    PGSQL       `yaml:"postgres_reporting"`
	LogLevel        string      `yaml:"log_level"`
	GinMode         string      `yaml:"gin_mode"`
	ServerAddress   string      `yaml:"server_address"`
}

type Permissions struct {
	AdminAccess string `yaml:"admin_access"`
	FavReports  string `yaml:"fav_reports"`
}

type MSSQL struct {
	Server   string `yaml:"server"`
	UserName string `yaml:"username"`
	Password string `yaml:"password"`
	Database string `yaml:"database"`
}

type PGSQL struct {
	Host     string `yaml:"host"`
	Port     int    `yaml:"port"`
	UserName string `yaml:"username"`
	Password string `yaml:"password"`
	DbName   string `yaml:"dbname"`
	SSLMode  string `yaml:"sslmode"`
}
