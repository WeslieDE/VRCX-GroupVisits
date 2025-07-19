<template>
    <div>
        <div style="display: flex; align-items: center; justify-content: space-between">
            <div>
                <el-button size="small" @click="showFriendExportDialog">{{ $t('view.favorite.export') }}</el-button>
                <el-button size="small" style="margin-left: 5px" @click="showFriendImportDialog">{{
                    $t('view.favorite.import')
                }}</el-button>
            </div>
            <div style="display: flex; align-items: center; font-size: 13px; margin-right: 10px">
                <span class="name" style="margin-right: 5px; line-height: 10px">{{ $t('view.favorite.sort_by') }}</span>
                <el-radio-group v-model="sortFav" @change="saveSortFavoritesOption">
                    <el-radio :label="false">{{
                        $t('view.settings.appearance.appearance.sort_favorite_by_name')
                    }}</el-radio>
                    <el-radio :label="true">{{
                        $t('view.settings.appearance.appearance.sort_favorite_by_date')
                    }}</el-radio>
                </el-radio-group>
            </div>
        </div>
        <span style="display: block; margin-top: 30px">{{ $t('view.favorite.avatars.vrchat_favorites') }}</span>
        <el-collapse style="border: 0">
            <el-collapse-item v-for="group in API.favoriteFriendGroups" :key="group.name">
                <template slot="title">
                    <span
                        style="font-weight: bold; font-size: 14px; margin-left: 10px"
                        v-text="group.displayName"></span>
                    <span style="color: #909399; font-size: 12px; margin-left: 10px"
                        >{{ group.count }}/{{ group.capacity }}</span
                    >
                    <el-tooltip placement="top" :content="$t('view.favorite.rename_tooltip')" :disabled="hideTooltips">
                        <el-button
                            size="mini"
                            icon="el-icon-edit"
                            circle
                            style="margin-left: 10px"
                            @click.stop="changeFavoriteGroupName(group)"></el-button>
                    </el-tooltip>
                    <el-tooltip placement="right" :content="$t('view.favorite.clear_tooltip')" :disabled="hideTooltips">
                        <el-button
                            size="mini"
                            icon="el-icon-delete"
                            circle
                            style="margin-left: 5px"
                            @click.stop="clearFavoriteGroup(group)"></el-button>
                    </el-tooltip>
                    <el-tooltip placement="right" :content="$t('view.favorite.invite_all_tooltip')" :disabled="hideTooltips">
                        <el-button
                            size="mini"
                            icon="el-icon-message"
                            circle
                            style="margin-left: 5px"
                            @click.stop="showInviteGroupDialog(group)"></el-button>
                    </el-tooltip>
                </template>
                <div v-if="group.count" class="x-friend-list" style="margin-top: 10px">
                    <FavoritesFriendItem
                        v-for="favorite in groupedByGroupKeyFavoriteFriends[group.key]"
                        :key="favorite.id"
                        style="display: inline-block; width: 300px; margin-right: 15px"
                        :favorite="favorite"
                        :edit-favorites-mode="editFavoritesMode"
                        :group="group"
                        @click="showUserDialog(favorite.id)" />
                </div>
                <div
                    v-else
                    style="
                        padding-top: 25px;
                        width: 100%;
                        display: flex;
                        align-items: center;
                        justify-content: center;
                        color: rgb(144, 147, 153);
                    ">
                    <span>No Data</span>
                </div>
            </el-collapse-item>
        </el-collapse>
        <FriendExportDialog
            :friend-export-dialog-visible.sync="friendExportDialogVisible"
            :favorite-friends="favoriteFriends" />
        <InviteDialog
            :invite-dialog="inviteDialog"
            :vip-friends="vipFriends"
            :online-friends="onlineFriends"
            :active-friends="activeFriends"
            :invite-message-table="inviteMessageTable"
            :upload-image="uploadImage"
            @closeInviteDialog="closeInviteDialog" />
    </div>
</template>

<script>
    import FavoritesFriendItem from './FavoritesFriendItem.vue';
    import FriendExportDialog from '../dialogs/FriendExportDialog.vue';
    import InviteDialog from '../../../components/dialogs/InviteDialog/InviteDialog.vue';
    import { favoriteRequest, worldRequest } from '../../../api';
    import { parseLocation, isRealInstance } from '../../../composables/instance/utils';

    export default {
        name: 'FavoritesFriendTab',
        components: { FriendExportDialog, FavoritesFriendItem, InviteDialog },
        inject: ['showUserDialog', 'API'],
        props: {
            favoriteFriends: Array,
            sortFavorites: Boolean,
            hideTooltips: Boolean,
            groupedByGroupKeyFavoriteFriends: Object,
            editFavoritesMode: Boolean
        },
        data() {
            return {
                friendExportDialogVisible: false,
                inviteDialog: {
                    visible: false,
                    loading: false,
                    worldId: '',
                    worldName: '',
                    userIds: [],
                    friendsInInstance: []
                }
            };
        },
        computed: {
            sortFav: {
                get() {
                    return this.sortFavorites;
                },
                set(value) {
                    this.$emit('update:sort-favorites', value);
                }
            },
            vipFriends() {
                return this.API.vipFriends;
            },
            onlineFriends() {
                return this.API.onlineFriends;
            },
            activeFriends() {
                return this.API.activeFriends;
            },
            inviteMessageTable() {
                return this.API.inviteMessageTable;
            },
            uploadImage() {
                return this.API.uploadImage;
            },
            lastLocation() {
                return this.API.lastLocation;
            }
        },
        methods: {
            showFriendExportDialog() {
                this.friendExportDialogVisible = true;
            },
            showFriendImportDialog() {
                this.$emit('show-friend-import-dialog');
            },
            saveSortFavoritesOption() {
                this.$emit('save-sort-favorites-option');
            },

            closeInviteDialog() {
                this.inviteDialog.visible = false;
            },

            showInviteGroupDialog(group) {
                const current = this.lastLocation.location;
                if (!isRealInstance(current)) {
                    return;
                }
                if (!this.API.checkCanInvite(current)) {
                    return;
                }
                const L = parseLocation(current);
                worldRequest
                    .getCachedWorld({
                        worldId: L.worldId
                    })
                    .then((args) => {
                        const D = this.inviteDialog;
                        D.userIds = [];
                        D.worldId = L.tag;
                        D.worldName = args.ref.name;
                        D.friendsInInstance = [];
                        const friendsInCurrentInstance = this.lastLocation.friendList;
                        for (const friend of friendsInCurrentInstance.values()) {
                            const ctx = this.API.friends.get(friend.userId);
                            if (ctx && ctx.ref) {
                                D.friendsInInstance.push(ctx);
                            }
                        }
                        const favs = this.groupedByGroupKeyFavoriteFriends[group.key] || [];
                        for (const fav of favs) {
                            if (fav.ref && fav.ref.location !== 'offline') {
                                D.userIds.push(fav.ref.id);
                            }
                        }
                        D.visible = true;
                    });
            },

            clearFavoriteGroup(ctx) {
                // FIXME: 메시지 수정
                this.$confirm('Continue? Clear Group', 'Confirm', {
                    confirmButtonText: 'Confirm',
                    cancelButtonText: 'Cancel',
                    type: 'info',
                    callback: (action) => {
                        if (action === 'confirm') {
                            favoriteRequest.clearFavoriteGroup({
                                type: ctx.type,
                                group: ctx.name
                            });
                        }
                    }
                });
            },
            changeFavoriteGroupName(group) {
                this.$emit('change-favorite-group-name', group);
            }
        }
    };
</script>
