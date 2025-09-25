package models

import "time"

type Report struct {
	Id                    *int       `json:"id" gorm:"column:Id; autoIncrement"`
	Text                  string     `json:"text" gorm:"column:Text"`
	URL                   string     `json:"-" gorm:"column:URL"`
	Visible               bool       `json:"visible" gorm:"column:Visible"`
	CreatedBy             string     `json:"-" gorm:"column:CreatedBy"`
	CreatedAt             time.Time  `json:"-" gorm:"column:CreatedAt"`
	UpdatedBy             string     `json:"-" gorm:"column:UpdatedBy"`
	UpdatedAt             time.Time  `json:"-" gorm:"column:UpdatedAt"`
	Alias                 string     `json:"alias" gorm:"column:Alias"`
	Description           string     `json:"description" gorm:"column:Description"`
	Owner                 *string    `json:"owner" gorm:"column:Owner"`
	OperationName         string     `json:"operation_name" gorm:"column:OperationName"`
	Type                  string     `json:"type" gorm:"-"`
	ParentID              *int       `json:"parentId" gorm:"-"`
	Data                  DataField  `json:"data" gorm:"-"`
	Categories            []Category `json:"-" gorm:"many2many:\"sys-reporting\".\"CategoryReports\";foreignKey:Id;joinForeignKey:ReportsId;References:Id;joinReferences:CategoriesId"`
	PrivateAlias          string     `json:"privateAlias" gorm:"column:PrivateAlias"`
	PrivateAliasExpiresAt *time.Time `json:"privateAliasExpiresAt" gorm:"column:PrivateAliasExpiresAt"`
}

type ReportWithParent struct {
	Report
	ParentID *int `json:"parentId" gorm:"column:ParentId"`
}

type FavoriteReportDTO struct {
	ReportId   int `json:"reportId" binding:"required"`
	CategoryId int `json:"categoryId" binding:"required"`
}

type FavoriteReportItem struct {
	Id          *int   `json:"id" gorm:"column:Id"`
	Text        string `json:"text" gorm:"column:Text"`
	Description string `json:"description" gorm:"column:Description"`
	Alias       string `json:"alias" gorm:"column:Alias"`
	ParentID    *int   `json:"parentId" gorm:"column:ParentId"`
	ParentAlias string `json:"parentAlias" gorm:"column:ParentAlias"`
}

func (Report) TableName() string {
	return "\"sys-reporting\".\"Reports\""
}

type DataField struct {
	URL string `json:"url"`
}

type FavoriteReport struct {
	ID         int    `gorm:"column:Id"`
	Login      string `gorm:"column:Login"`
	ReportId   int    `gorm:"column:ReportId"`
	CategoryId int    `gorm:"column:CategoryId"`
}

func (FavoriteReport) TableName() string {
	return "\"sys-reporting\".\"FavoriteReports\""
}

type TrackVisitDTO struct {
	ReportId   int `json:"reportId" binding:"required"`
	CategoryId int `json:"categoryId" binding:"required"`
}

type VisitHistory struct {
	Dt         time.Time `gorm:"column:Dt;<-:false"`
	ReportId   int       `gorm:"column:ReportId"`
	CategoryId int       `gorm:"column:CategoryId"`
	Login      string    `gorm:"column:Login"`
	IpAddress  string    `gorm:"column:IpAddress"`
}

func (VisitHistory) TableName() string {
	return "\"sys-reporting\".\"VisitHistory\""
}

type VisitedReport struct {
	Name          string `json:"name"`
	Alias         string `json:"alias"`
	CategoryAlias string `json:"category_alias"`
}

type UpdateReport struct {
	Id                    int     `json:"id" gorm:"column:Id"`
	Text                  string  `json:"text" gorm:"column:Text"`
	Description           string  `json:"description" gorm:"column:Description"`
	Owner                 *string `json:"owner" gorm:"column:Owner"`
	Visible               bool    `json:"visible" gorm:"column:Visible"`
	URL                   string  `json:"url" gorm:"column:URL"`
	OperationName         string  `json:"operation_name" gorm:"column:OperationName"`
	PrivateAlias          string  `json:"privateAlias"`
	PrivateAliasExpiresAt string  `json:"privateAliasExpiresAt"`
}

func (UpdateReport) TableName() string {
	return "\"sys-reporting\".\"Reports\""
}

type CreateReport struct {
	Text                  string  `json:"text"`
	Description           string  `json:"description"`
	Alias                 string  `json:"alias"`
	Owner                 *string `json:"owner"`
	ParentId              int     `json:"parentId"`
	URL                   string  `json:"url"`
	Visible               bool    `json:"visible"`
	OperationName         string  `json:"operation_name"`
	PrivateAlias          string  `json:"privateAlias"`
	PrivateAliasExpiresAt string  `json:"privateAliasExpiresAt"`
}

func (CreateReport) TableName() string {
	return "\"sys-reporting\".\"Reports\""
}
