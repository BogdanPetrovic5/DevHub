export interface CreateRepoRequest{
    name:string,
    description:string,
    isPrivate:boolean
}
export interface RepoResponse{
    success:boolean,
    message:string
}