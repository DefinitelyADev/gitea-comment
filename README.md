# Gitea Comment

A Drone plugin to post comments on a Gitea Pull Request

Docker Hub: https://hub.docker.com/r/tsakidev/giteacomment

Example reference for pull request with static string:

```yml
steps:
- name: post-to-gitea-pr
  image: tsakidev/giteacomment:latest
  settings:
    gitea_token:
      from_secret: gitea_token
    gitea_base_url: http://gitea.example.com
    comment: "Hello from Drone"
  when:
    status: [ failure ]
    event: pull_request
```

Example reference for pull request with input from file:

```yml
steps:
- name: post-to-gitea-pr
  image: tsakidev/giteacomment:latest
  settings:
    gitea_token:
      from_secret: gitea_token
    gitea_base_url: http://gitea.example.com
    comment_title: "My Title"
    comment_from_file: "/path/to/file.txt"
  when:
    status: [ failure ]
    event: pull_request
```
