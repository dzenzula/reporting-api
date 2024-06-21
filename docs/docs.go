// Package docs Code generated by swaggo/swag. DO NOT EDIT
package docs

import "github.com/swaggo/swag"

const docTemplate = `{
    "schemes": {{ marshal .Schemes }},
    "swagger": "2.0",
    "info": {
        "description": "{{escape .Description}}",
        "title": "{{.Title}}",
        "contact": {},
        "version": "{{.Version}}"
    },
    "host": "{{.Host}}",
    "basePath": "{{.BasePath}}",
    "paths": {
        "/api/Authorization/GetCurrentUserInfo": {
            "get": {
                "consumes": [
                    "application/json"
                ],
                "produces": [
                    "application/json"
                ],
                "tags": [
                    "Authorization"
                ],
                "responses": {
                    "200": {
                        "description": "OK",
                        "schema": {
                            "$ref": "#/definitions/models.AuthUserData"
                        }
                    }
                }
            }
        },
        "/api/Authorization/LogInAuthorization": {
            "post": {
                "consumes": [
                    "application/json"
                ],
                "produces": [
                    "application/json"
                ],
                "tags": [
                    "Authorization"
                ],
                "parameters": [
                    {
                        "description": "Данные пользователя",
                        "name": "userdata",
                        "in": "body",
                        "required": true,
                        "schema": {
                            "$ref": "#/definitions/models.UserData"
                        }
                    }
                ],
                "responses": {
                    "200": {
                        "description": "OK",
                        "schema": {
                            "$ref": "#/definitions/models.AuthUserData"
                        }
                    }
                }
            }
        },
        "/api/Authorization/LogOutAuthorization": {
            "post": {
                "consumes": [
                    "application/json"
                ],
                "produces": [
                    "application/json"
                ],
                "tags": [
                    "Authorization"
                ],
                "responses": {
                    "200": {
                        "description": "OK"
                    }
                }
            }
        },
        "/api/Categories": {
            "get": {
                "description": "Получение списка категорий",
                "consumes": [
                    "application/json"
                ],
                "produces": [
                    "application/json"
                ],
                "tags": [
                    "Categories"
                ],
                "summary": "Get Categories",
                "responses": {
                    "200": {
                        "description": "OK",
                        "schema": {
                            "type": "array",
                            "items": {
                                "$ref": "#/definitions/models.GetCategory"
                            }
                        }
                    }
                }
            },
            "put": {
                "description": "Обновить категорию",
                "consumes": [
                    "application/json"
                ],
                "produces": [
                    "application/json"
                ],
                "tags": [
                    "Categories"
                ],
                "summary": "Update Category",
                "parameters": [
                    {
                        "description": "Обновленная категория",
                        "name": "data",
                        "in": "body",
                        "required": true,
                        "schema": {
                            "$ref": "#/definitions/models.UpdateCategory"
                        }
                    }
                ],
                "responses": {
                    "200": {
                        "description": "OK"
                    }
                }
            },
            "post": {
                "description": "Создать категорию",
                "consumes": [
                    "application/json"
                ],
                "produces": [
                    "application/json"
                ],
                "tags": [
                    "Categories"
                ],
                "summary": "Insert Category",
                "parameters": [
                    {
                        "description": "Новая категория",
                        "name": "data",
                        "in": "body",
                        "required": true,
                        "schema": {
                            "$ref": "#/definitions/models.InsertCategory"
                        }
                    }
                ],
                "responses": {
                    "200": {
                        "description": "OK"
                    }
                }
            },
            "delete": {
                "description": "Удалить категорию",
                "consumes": [
                    "application/json"
                ],
                "produces": [
                    "application/json"
                ],
                "tags": [
                    "Categories"
                ],
                "summary": "Delete Category",
                "parameters": [
                    {
                        "type": "integer",
                        "description": "Id категории",
                        "name": "id",
                        "in": "query",
                        "required": true
                    }
                ],
                "responses": {
                    "200": {
                        "description": "OK"
                    }
                }
            }
        },
        "/api/Categories/GetCategoriesForAdmin": {
            "get": {
                "description": "Получение списка категорий для администратора",
                "consumes": [
                    "application/json"
                ],
                "produces": [
                    "application/json"
                ],
                "tags": [
                    "Categories"
                ],
                "summary": "Get Categories for Admin",
                "responses": {
                    "200": {
                        "description": "OK",
                        "schema": {
                            "type": "array",
                            "items": {
                                "$ref": "#/definitions/models.GetCategory"
                            }
                        }
                    }
                }
            }
        },
        "/api/Categories/UpdateCategoryParent": {
            "put": {
                "description": "Обновить родительскую категорию",
                "consumes": [
                    "application/json"
                ],
                "produces": [
                    "application/json"
                ],
                "tags": [
                    "Categories"
                ],
                "summary": "Update Parent Category",
                "parameters": [
                    {
                        "description": "Обновленный родительский айди",
                        "name": "data",
                        "in": "body",
                        "required": true,
                        "schema": {
                            "$ref": "#/definitions/models.UpdateCategoryParent"
                        }
                    }
                ],
                "responses": {
                    "200": {
                        "description": "OK"
                    }
                }
            }
        },
        "/api/CategoryReports": {
            "put": {
                "description": "Update the category of a report",
                "consumes": [
                    "application/json"
                ],
                "produces": [
                    "application/json"
                ],
                "tags": [
                    "Reports"
                ],
                "summary": "Update category reports",
                "parameters": [
                    {
                        "description": "Category data",
                        "name": "data",
                        "in": "body",
                        "required": true,
                        "schema": {
                            "$ref": "#/definitions/models.UpdateCategoryParent"
                        }
                    }
                ],
                "responses": {
                    "200": {
                        "description": "ok",
                        "schema": {
                            "type": "string"
                        }
                    }
                }
            }
        },
        "/api/FavoriteReports/AddReport": {
            "post": {
                "description": "Add a report to favorites by report ID",
                "produces": [
                    "application/json"
                ],
                "tags": [
                    "FavoriteReports"
                ],
                "summary": "Add a report to favorites",
                "parameters": [
                    {
                        "type": "integer",
                        "description": "Report ID",
                        "name": "id",
                        "in": "query",
                        "required": true
                    }
                ],
                "responses": {
                    "200": {
                        "description": "OK"
                    }
                }
            }
        },
        "/api/FavoriteReports/DeleteReport": {
            "delete": {
                "description": "Remove a report from favorites by report ID",
                "produces": [
                    "application/json"
                ],
                "tags": [
                    "FavoriteReports"
                ],
                "summary": "Remove a report from favorites",
                "parameters": [
                    {
                        "type": "integer",
                        "description": "Report ID",
                        "name": "id",
                        "in": "query",
                        "required": true
                    }
                ],
                "responses": {
                    "200": {
                        "description": "OK"
                    }
                }
            }
        },
        "/api/FavoriteReports/GetReports": {
            "get": {
                "description": "Get a list of all favorite reports",
                "produces": [
                    "application/json"
                ],
                "tags": [
                    "FavoriteReports"
                ],
                "summary": "Get all favorite reports",
                "responses": {
                    "200": {
                        "description": "OK",
                        "schema": {
                            "type": "array",
                            "items": {
                                "$ref": "#/definitions/models.FavoriteReport"
                            }
                        }
                    }
                }
            }
        },
        "/api/Reports": {
            "get": {
                "description": "Get a list of all reports",
                "produces": [
                    "application/json"
                ],
                "tags": [
                    "Reports"
                ],
                "summary": "Get all reports",
                "responses": {
                    "200": {
                        "description": "OK",
                        "schema": {
                            "type": "array",
                            "items": {
                                "$ref": "#/definitions/models.Report"
                            }
                        }
                    }
                }
            },
            "put": {
                "description": "Update an existing report",
                "consumes": [
                    "application/json"
                ],
                "produces": [
                    "application/json"
                ],
                "tags": [
                    "Reports"
                ],
                "summary": "Update a report",
                "parameters": [
                    {
                        "description": "Report to update",
                        "name": "report",
                        "in": "body",
                        "required": true,
                        "schema": {
                            "$ref": "#/definitions/models.UpdateReport"
                        }
                    }
                ],
                "responses": {
                    "200": {
                        "description": "ok",
                        "schema": {
                            "type": "string"
                        }
                    }
                }
            },
            "post": {
                "description": "Create a new report",
                "consumes": [
                    "application/json"
                ],
                "produces": [
                    "application/json"
                ],
                "tags": [
                    "Reports"
                ],
                "summary": "Create a new report",
                "parameters": [
                    {
                        "description": "Report to create",
                        "name": "report",
                        "in": "body",
                        "required": true,
                        "schema": {
                            "$ref": "#/definitions/models.CreateReport"
                        }
                    }
                ],
                "responses": {
                    "200": {
                        "description": "ok",
                        "schema": {
                            "type": "string"
                        }
                    }
                }
            },
            "delete": {
                "description": "Remove a report from a category",
                "consumes": [
                    "application/json"
                ],
                "produces": [
                    "application/json"
                ],
                "tags": [
                    "Reports"
                ],
                "summary": "Remove a report",
                "parameters": [
                    {
                        "type": "integer",
                        "description": "Report ID",
                        "name": "reportId",
                        "in": "query",
                        "required": true
                    },
                    {
                        "description": "Category ID",
                        "name": "categoryId",
                        "in": "body",
                        "required": true,
                        "schema": {
                            "type": "integer"
                        }
                    }
                ],
                "responses": {
                    "200": {
                        "description": "ok",
                        "schema": {
                            "type": "string"
                        }
                    }
                }
            }
        },
        "/api/Reports/AddReportRelation": {
            "post": {
                "description": "Add a report to a category",
                "consumes": [
                    "application/json"
                ],
                "produces": [
                    "application/json"
                ],
                "tags": [
                    "Reports"
                ],
                "summary": "Add report relation",
                "parameters": [
                    {
                        "type": "integer",
                        "description": "Report ID",
                        "name": "reportId",
                        "in": "query",
                        "required": true
                    },
                    {
                        "description": "Category ID",
                        "name": "categoryId",
                        "in": "body",
                        "required": true,
                        "schema": {
                            "type": "integer"
                        }
                    }
                ],
                "responses": {
                    "200": {
                        "description": "ok",
                        "schema": {
                            "type": "string"
                        }
                    }
                }
            }
        }
    },
    "definitions": {
        "models.AuthUserData": {
            "type": "object",
            "properties": {
                "authStatus": {
                    "type": "integer"
                },
                "displayName": {
                    "type": "string"
                },
                "domain": {
                    "type": "string"
                },
                "name": {
                    "type": "string"
                },
                "permission": {
                    "type": "array",
                    "items": {
                        "$ref": "#/definitions/models.MyPermission"
                    }
                },
                "type": {
                    "type": "string"
                }
            }
        },
        "models.CreateReport": {
            "type": "object",
            "properties": {
                "alias": {
                    "type": "string"
                },
                "description": {
                    "type": "string"
                },
                "operation_name": {
                    "type": "string"
                },
                "owner": {
                    "type": "string"
                },
                "parentId": {
                    "type": "integer"
                },
                "text": {
                    "type": "string"
                },
                "url": {
                    "type": "string"
                },
                "visible": {
                    "type": "boolean"
                }
            }
        },
        "models.DataField": {
            "type": "object",
            "properties": {
                "url": {
                    "type": "string"
                }
            }
        },
        "models.FavoriteReport": {
            "type": "object",
            "properties": {
                "id": {
                    "type": "integer"
                },
                "login": {
                    "type": "string"
                },
                "reportId": {
                    "type": "integer"
                }
            }
        },
        "models.GetCategory": {
            "type": "object",
            "properties": {
                "description": {
                    "type": "string"
                },
                "id": {
                    "type": "integer"
                },
                "parentId": {
                    "type": "integer"
                },
                "text": {
                    "type": "string"
                },
                "type": {
                    "type": "string"
                },
                "visible": {
                    "type": "boolean"
                }
            }
        },
        "models.InsertCategory": {
            "type": "object",
            "properties": {
                "description": {
                    "type": "string"
                },
                "parentId": {
                    "type": "integer"
                },
                "text": {
                    "type": "string"
                },
                "visible": {
                    "type": "boolean"
                }
            }
        },
        "models.MyPermission": {
            "type": "object",
            "properties": {
                "name": {
                    "type": "string"
                },
                "permission": {
                    "type": "integer"
                }
            }
        },
        "models.Report": {
            "type": "object",
            "properties": {
                "alias": {
                    "type": "string"
                },
                "data": {
                    "$ref": "#/definitions/models.DataField"
                },
                "description": {
                    "type": "string"
                },
                "id": {
                    "type": "integer"
                },
                "operation_name": {
                    "type": "string"
                },
                "owner": {
                    "type": "string"
                },
                "parentId": {
                    "type": "integer"
                },
                "text": {
                    "type": "string"
                },
                "type": {
                    "type": "string"
                },
                "visible": {
                    "type": "boolean"
                }
            }
        },
        "models.UpdateCategory": {
            "type": "object",
            "properties": {
                "description": {
                    "type": "string"
                },
                "id": {
                    "type": "integer"
                },
                "text": {
                    "type": "string"
                },
                "visible": {
                    "type": "boolean"
                }
            }
        },
        "models.UpdateCategoryParent": {
            "type": "object",
            "properties": {
                "fromCat": {
                    "type": "integer"
                },
                "id": {
                    "type": "integer"
                },
                "toCat": {
                    "type": "integer"
                }
            }
        },
        "models.UpdateReport": {
            "type": "object",
            "properties": {
                "description": {
                    "type": "string"
                },
                "id": {
                    "type": "integer"
                },
                "operation_name": {
                    "type": "string"
                },
                "owner": {
                    "type": "string"
                },
                "text": {
                    "type": "string"
                },
                "url": {
                    "type": "string"
                },
                "visible": {
                    "type": "boolean"
                }
            }
        },
        "models.UserData": {
            "type": "object",
            "properties": {
                "domain": {
                    "type": "string"
                },
                "login": {
                    "type": "string"
                },
                "password": {
                    "type": "string"
                }
            }
        }
    }
}`

// SwaggerInfo holds exported Swagger Info so clients can modify it
var SwaggerInfo = &swag.Spec{
	Version:          "1.0",
	Host:             "",
	BasePath:         "/reporting-api/",
	Schemes:          []string{},
	Title:            "Reporting API",
	Description:      "This is a sample server for reporting API.",
	InfoInstanceName: "swagger",
	SwaggerTemplate:  docTemplate,
	LeftDelim:        "{{",
	RightDelim:       "}}",
}

func init() {
	swag.Register(SwaggerInfo.InstanceName(), SwaggerInfo)
}
