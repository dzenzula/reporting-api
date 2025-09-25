package models

import (
	"errors"
	"regexp"
	"strings"
)

func ValidateAlias(alias string) error {
	if strings.TrimSpace(alias) == "" {
		return errors.New("Alias cannot be empty")
	}

	if len(alias) < 3 || len(alias) > 50 {
		return errors.New("alias length must be between 3 and 50 characters")
	}

	var aliasRegex = regexp.MustCompile(`^[a-zA-Z0-9]+(?:[-_][a-zA-Z0-9]+)*$`)
	if !aliasRegex.MatchString(alias) {
		return errors.New("alias can contain letters, digits; '-' and '_' only as single separators")
	}
	return nil
}
