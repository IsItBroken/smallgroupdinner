terraform {
  required_version = "~> 1.5.0"
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "= 3.109.0"
    }
    azuread = {
      source  = "hashicorp/azuread"
      version = "= 2.47.0"
    }
    time = {
      source  = "hashicorp/time"
      version = "= 0.10.0"

    }
    auth0 = {
      source  = "auth0/auth0"
      version = ">= 1.2.0" # Refer to docs for latest version
    }
  }
  backend "azurerm" {
    resource_group_name  = "Devops-Rg"
    storage_account_name = "sgdterraform"
    container_name       = "base-infrastructure"
    key                  = "terraform.tfstate"
  }
}

provider "azurerm" {
  skip_provider_registration = false # This is only required when the User, Service Principal, or Identity running Terraform lacks the permissions to register Azure Resource Providers.
  features {}
  subscription_id = var.subscription_id
}
