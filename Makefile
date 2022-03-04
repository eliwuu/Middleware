all:
	@./run 10
	@./run-http2 10
	@./run 100
	@./run-http2 100
	@echo

.PHONY: all
