variable "subscription_id" {
  description = "Subscription ID"
  type        = string
}

variable "environment" {
  description = "The environment in which the infrastructure is being deployed"
  type        = string

  validation {
    condition     = contains(["development", "production"], var.environment)
    error_message = "The environment must be one of: development, staging, or production."
  }
}

variable "location" {
  description = "The location/region in which the infrastructure is being deployed"
  type        = string

  validation {
    condition     = contains(["East US"], var.location)
    error_message = "The location must be East US."
  }
}
