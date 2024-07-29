package models

import "time"

type Category struct {
	Id          int       `gorm:"column:Id; primary_key; autoIncrement"`
	ParentId    *int      `gorm:"column:ParentId"`
	Text        string    `gorm:"column:Text"`
	Visible     bool      `gorm:"column:Visible"`
	CreatedBy   string    `gorm:"column:CreatedBy"`
	CreatedAt   time.Time `gorm:"column:CreatedAt"`
	UpdatedBy   string    `gorm:"column:UpdatedBy"`
	UpdatedAt   time.Time `gorm:"column:UpdatedAt"`
	Description string    `gorm:"column:Description"`
	Reports     []Report  `gorm:"many2many:\"sys-reporting\".\"CategoryReports\";foreignKey:Id;joinForeignKey:CategoriesId;References:Id;joinReferences:ReportsId"`
}

func (Category) TableName() string {
	return "\"sys-reporting\".\"Categories\""
}

type GetCategory struct {
	Id          *int   `json:"id" gorm:"column:Id"`
	Text        string `json:"text" gorm:"size:150;not null;column:Text"`
	Description string `json:"description" gorm:"size:150;column:Description"`
	Type        string `json:"type" gorm:"-"`
	ParentId    *int   `json:"parentId" gorm:"column:ParentId"`
	Visible     bool   `json:"visible" gorm:"column:Visible"`
}

func (GetCategory) TableName() string {
	return "\"sys-reporting\".\"Categories\""
}

type UpdateCategory struct {
	Id          int    `json:"id" gorm:"column:Id"`
	Text        string `json:"text" gorm:"column:Text"`
	Description string `json:"description" gorm:"column:Description"`
	Visible     bool   `json:"visible" gorm:"column:Visible"`
}

func (UpdateCategory) TableName() string {
	return "\"sys-reporting\".\"Categories\""
}

type InsertCategory struct {
	Text        string `json:"text" gorm:"column:Text"`
	Description string `json:"description" gorm:"column:Description"`
	ParentId    *int   `json:"parentId" gorm:"column:ParentId"`
	Visible     bool   `json:"visible" gorm:"column:Visible"`
}

func (InsertCategory) TableName() string {
	return "\"sys-reporting\".\"Categories\""
}

type UpdateCategoryParent struct {
	Id      int `json:"id"`
	FromCat int `json:"fromCat"`
	ToCat   int `json:"toCat"`
}

func (UpdateCategoryParent) TableName() string {
	return "\"sys-reporting\".\"Categories\""
}

type CategoryReports struct {
	ReportsId    int `gorm:"column:ReportsId;primaryKey"`
	CategoriesId int `gorm:"column:CategoriesId;primaryKey"`
}

func (CategoryReports) TableName() string {
	return "\"sys-reporting\".\"CategoryReports\""
}
