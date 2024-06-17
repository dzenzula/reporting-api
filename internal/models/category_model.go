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
}

type GetCategory struct {
	Id          *int   `json:"id" gorm:"column:Id"`
	Text        string `json:"text" gorm:"size:150;not null;column:Text"`
	Description string `json:"description" gorm:"size:150;column:Description"`
	Type        string `json:"type" gorm:"-"`
	ParentId    *int   `json:"parentId" gorm:"column:ParentId"`
	Visible     bool   `json:"visible" gorm:"column:Visible"`
}

type UpdateCategory struct {
	Id          int   `json:"id" gorm:"column:Id"`
	Text        string `json:"text" gorm:"column:Text"`
	Description string `json:"description" gorm:"column:Description"`
	Visible     bool   `json:"visible" gorm:"column:Visible"`
}

type InsertCategory struct {
	Text        string `json:"text" gorm:"column:Text"`
	Description string `json:"description" gorm:"column:Description"`
	ParentId    *int   `json:"parentId" gorm:"column:ParentId"`
	Visible     bool   `json:"visible" gorm:"column:Visible"`
}

type UpdateCategoryParent struct {
	Id      int `json:"id"`
	FromCat int `json:"fromCat"`
	ToCat   int `json:"toCat"`
}
