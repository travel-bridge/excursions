.PHONY: all

REGISTRY?=local
BUILD_NUMBER?=latest
REVISION?=revision-not-specified
TEST_NETWORK?=host
TEST_ENVIRONMENT?=docker

build:
	@echo build api:
	docker build . --rm  --tag ${REGISTRY}/excursions_api:$(BUILD_NUMBER) --target api --build-arg revision='${REVISION}'

	@echo build migrator:
	docker build . --rm  --tag ${REGISTRY}/excursions_migrator:$(BUILD_NUMBER) --target migrator --build-arg revision='${REVISION}'

	@echo build tests:
		docker build . --rm  --tag ${REGISTRY}/excursions_tests:$(BUILD_NUMBER) --target tests

test:
	@echo run tests:
	docker run --rm --network='${TEST_NETWORK}' -e ASPNETCORE_ENVIRONMENT=${TEST_ENVIRONMENT} ${REGISTRY}/excursions_tests:$(BUILD_NUMBER)