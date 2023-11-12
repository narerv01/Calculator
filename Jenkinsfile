pipeline { 
	agent any
	triggers {
		pollSCM("* * * * *")
	}
	stages {
		stage("Build"){
			steps {
				bat "docker compose build"
			} 
		} 
		stage("Test"){
			steps {
				bat "docker compose up test-service"
			}
		}  
		stage("Deliver"){
			steps {
				bat "docker compose push" 
			}
		} 
		stage("Deploy to Swarm") {
            steps {
                bat "docker stack deploy --compose-file docker-compose.yml test_service_image"  
            }
        }
	}
}