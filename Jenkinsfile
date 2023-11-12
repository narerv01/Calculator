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
					withCredentials([usernamePassword(credentialsId: 'DockerHub', usernameVariable: 'USERNAME', passwordVariable: 'PASSWORD')]) { 
					bat 'docker login -u ${USERNAME} -p ${PASSWORD}' 
					echo USERNAME 
					echo "username is $USERNAME"
					echo PASSWORD 
					echo "password is $PASSWORD"
				}
			}
		} 
	}
}