// // Basic Web Server
// package main

// import (
// 	"encoding/json"
// 	"flag"
// 	"fmt"
// 	"log"
// 	"net/http"
// )

// const version = "1.0.0"

// type AppStatus struct {
// 	Status      string `json:"status"`
// 	Environment string `json:"environment"`
// 	Version     string `json:"version"`
// }

// func main() {
// 	var port int
// 	flag.IntVar(&port, "port", 4001, "Server port to listen")
// 	flag.Parse()

// 	http.HandleFunc("/hc", func(w http.ResponseWriter, r *http.Request) {
// 		currentStatus := AppStatus{
// 			Status:      "Available",
// 			Environment: "Development",
// 			Version:     version,
// 		}

// 		js, err := json.MarshalIndent(currentStatus, "", "\t")
// 		if err != nil {
// 			log.Println(err)
// 		}

// 		w.Header().Set("Content-Type", "application/json")
// 		w.WriteHeader(http.StatusOK)
// 		w.Write(js)
// 	})

// 	log.Println("Starting Server on Port", port)
// 	err := http.ListenAndServe(fmt.Sprintf(":%d", port), nil)
// 	if err != nil {
// 		log.Println(err)
// 	}
// }