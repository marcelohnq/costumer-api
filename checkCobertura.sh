#!/usr/bin/env sh
coberturaXML=$1
COVERAGE=$(grep -oE '<coverage.*branch-rate' $coberturaXML | grep -oE '".*"' | tr -d '"')

# if less than 0.9 ? 1 : 0
exit $(echo "$COVERAGE < 0.9" | bc -l)