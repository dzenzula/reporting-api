package models

type Config struct {
	Permissions     Permissions `yaml:"permissions"`
	MssqlConnection MSSQL       `yaml:"mssql"`
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
	User     string `yaml:"user"`
	Password string `yaml:"password"`
	Database string `yaml:"database"`
}
