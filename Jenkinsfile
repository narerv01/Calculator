pipeline { 
	agent any
	triggers {
		pollSCM("* * * * *")
	}
	stages {
		stage("Build"){
			steps {
				bat ""
			} 
		} 
		stage("Test"){
			steps {
				bat "docker compose up test-service"
			}
		}  
		stage("Deliver"){
			steps {
					 
				bat 'docker login localhost:8080'
				bat 'docker push narerv01/calculator:test_service_tag' 	
				 
			}
		} 
		stage("Deploy to Swarm") {
            steps {
                bat "docker stack deploy --compose-file docker-compose.yml test_service_image"  
            }
        }
	}
}