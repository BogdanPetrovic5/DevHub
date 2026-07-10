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
    createdAt: string,
    ownerUsername: string
}

export interface RepoLanguageDto {
    language: string,
    fileCount: number,
    percentage: number
}

export interface RepoCommitSummaryDto {
    id: string,
    message: string,
    authorUsername: string,
    createdAt: string,
    shortHash: string
}

export interface TreeItemDto {
    name: string,
    path: string,
    type: 'tree' | 'blob',
    lastCommitMessage: string,
    lastCommitAt: string
}

export interface RepoDetailDto {
    id: string,
    name: string,
    description: string | null,
    isPrivate: boolean,
    createdAt: string,
    ownerUsername: string,
    languages: RepoLanguageDto[],
    latestCommit: RepoCommitSummaryDto | null,
    tree: TreeItemDto[]
}
export interface RepoFileContentDto {
  path: string;
  content: string;
  language: string;
}

export interface CommitFilesDto {
    authorUsername: string,
    commitMessage: string,
    createdAt: string,
    files: RepoCommitFileDto[]

}
export interface RepoCommitFileDto{
    path:string,
    content:string,
    changeType:'Added'|'Modified'|'Deleted'
}