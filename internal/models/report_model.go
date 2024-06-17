package models

type Report struct {
	ID            int       `json:"id" gorm:"column:Id"`
	Text          string    `json:"text" gorm:"column:Text"`
	URL           string    `json:"-" gorm:"column:URL"`
	Visible       bool      `json:"visible" gorm:"column:Visible"`
	CreatedBy     string    `json:"-" gorm:"column:CreatedBy"`
	CreatedAt     string    `json:"-" gorm:"column:CreatedAt"`
	UpdatedBy     string    `json:"-" gorm:"column:UpdatedBy"`
	UpdatedAt     string    `json:"-" gorm:"column:UpdatedAt"`
	Alias         string    `json:"alias" gorm:"column:Alias"`
	Description   string    `json:"description" gorm:"column:Description"`
	Owner         string    `json:"owner" gorm:"column:CreatedBy"`
	OperationName string    `json:"operation_name" gorm:"column:Operation_name"`
	Type          string    `json:"type" gorm:"-"`
	ParentID      int       `json:"parentId" gorm:"-"`
	Data          DataField `json:"data" gorm:"-"`
}

type DataField struct {
	URL string `json:"url"`
}

type FavoriteReport struct {
	ID       int    `gorm:"column:Id"`
	Login    string `gorm:"column:Login"`
	ReportId int    `gorm:"column:ReportId"`
}
