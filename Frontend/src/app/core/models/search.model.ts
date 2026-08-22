export interface RepoSearchItemDto {
    ownerUsername: string,
    repoName: string
}

export interface UserSearchItemDto {
    username: string
}

export interface SearchResultDto {
    repositories: RepoSearchItemDto[],
    users: UserSearchItemDto[]
}
