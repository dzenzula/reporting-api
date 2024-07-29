package models

import "time"

type Report struct {
	Id            *int       `json:"id" gorm:"column:Id; autoIncrement"`
	Text          string     `json:"text" gorm:"column:Text"`
	URL           string     `json:"-" gorm:"column:URL"`
	Visible       bool       `json:"visible" gorm:"column:Visible"`
	CreatedBy     string     `json:"-" gorm:"column:CreatedBy"`
	CreatedAt     time.Time  `json:"-" gorm:"column:CreatedAt"`
	UpdatedBy     string     `json:"-" gorm:"column:UpdatedBy"`
	UpdatedAt     time.Time  `json:"-" gorm:"column:UpdatedAt"`
	Alias         string     `json:"alias" gorm:"column:Alias"`
	Description   string     `json:"description" gorm:"column:Description"`
	Owner         string     `json:"owner" gorm:"column:Owner"`
	OperationName string     `json:"operation_name" gorm:"column:OperationName"`
	Type          string     `json:"type" gorm:"-"`
	ParentID      *int       `json:"parentId" gorm:"-"`
	Data          DataField  `json:"data" gorm:"-"`
	Categories    []Category `json:"-" gorm:"many2many:\"sys-reporting\".\"CategoryReports\";foreignKey:Id;joinForeignKey:ReportsId;References:Id;joinReferences:CategoriesId"`
}

type ReportWithParent struct {
	Report
	ParentID *int `json:"parentId" gorm:"column:ParentId"`
}

func (Report) TableName() string {
	return "\"sys-reporting\".\"Reports\""
}

type DataField struct {
	URL string `json:"url"`
}

type FavoriteReport struct {
	ID       int    `gorm:"column:Id"`
	Login    string `gorm:"column:Login"`
	ReportId int    `gorm:"column:ReportId"`
}

func (FavoriteReport) TableName() string {
	return "\"sys-reporting\".\"FavoriteReports\""
}

type UpdateReport struct {
	Id            int    `json:"id" gorm:"column:Id"`
	Text          string `json:"text" gorm:"column:Text"`
	Description   string `json:"description" gorm:"column:Description"`
	Owner         string `json:"owner" gorm:"column:Owner"`
	Visible       bool   `json:"visible" gorm:"column:Visible"`
	URL           string `json:"url" gorm:"column:URL"`
	OperationName string `json:"operation_name" gorm:"column:OperationName"`
}

func (UpdateReport) TableName() string {
	return "\"sys-reporting\".\"Reports\""
}

type CreateReport struct {
	Text          string `json:"text"`
	Description   string `json:"description"`
	Alias         string `json:"alias"`
	Owner         string `json:"owner"`
	ParentId      int    `json:"parentId"`
	URL           string `json:"url"`
	Visible       bool   `json:"visible"`
	OperationName string `json:"operation_name"`
}

func (CreateReport) TableName() string {
	return "\"sys-reporting\".\"Reports\""
}
