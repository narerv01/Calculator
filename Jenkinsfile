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
					withCredentials([usernamePassword(credentialsId: 'DockerHub', usernameVariable: 'USERNAME', passwordVariable: 'PASSWORD')]) { 
					bat 'docker login -u %USERNAME% -p %PASSWORD%'   
					bat 'docker push narerv01/calculator:test_service_tag'
				}
			}
		} 
		stage("Deploy to Swarm") {
            steps {
                bat "docker stack deploy --compose-file docker-compose.yml test_service_image"  
            }
        }
	}
}