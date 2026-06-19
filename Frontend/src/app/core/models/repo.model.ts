export interface CreateRepoRequest {
    name: string,
    description: string,
    isPrivate: boolean
}

export interface RepoResponse {
    success: boolean,
    message: string
}

export interface RepoDto {
    id: string,
    name: string,
    description: string | null,
    isPrivate: boolean,
    createdAt: string
}