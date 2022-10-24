### STAGE 1: Build ###
FROM node:latest AS build
WORKDIR /usr/src/app
COPY package.json package-lock.json ./
RUN npm install
RUN npm install validator
COPY . .
RUN npm run build

### STAGE 2: Run ###
FROM nginx:1.23.1-alpine
#COPY nginx.conf /etc/nginx/nginx.conf
COPY --from=build /usr/src/app/dist/azngwebapp /usr/share/nginx/html


#docker build -t anzwebappimage .

#docker run --name anzwebappimage-container -d -p 3800:80 anzwebappimage