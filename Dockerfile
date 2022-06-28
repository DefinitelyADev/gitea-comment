FROM mcr.microsoft.com/dotnet/core/runtime:3.1.8-buster-slim
ADD artifacts/* /bin/
RUN chmod +x /bin/IT.GiteaComment
CMD "/bin/IT.GiteaComment"
