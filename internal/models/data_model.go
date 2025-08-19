package models

type PathDataResponse struct {
	Categories []GetCategory `json:"categories"`
	Reports    []Report      `json:"reports"`
	IsPrivate  bool          `json:"isPrivate"`
}
