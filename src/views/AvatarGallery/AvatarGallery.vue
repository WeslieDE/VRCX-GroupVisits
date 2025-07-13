<template>
    <div v-show="menuActiveIndex === 'avatarGallery'" class="x-container">
        <div style="margin-bottom: 10px; display: flex; align-items: center">
            <el-select
                v-model="selectedTags"
                multiple
                clearable
                collapse-tags
                placeholder="Filter tags"
                style="flex: 1">
                <el-option v-for="tag in allTags" :key="tag" :label="tag" :value="tag" />
            </el-select>
            <el-input
                v-model="searchQuery"
                placeholder="Search name"
                clearable
                style="flex: none; width: 150px; margin-left: 10px" />
        </div>
        <div class="avatar-gallery">
            <AvatarCard v-for="avatar in filteredAvatars" :key="avatar.id" :avatar="avatar" />
        </div>
    </div>
</template>

<script>
    import AvatarCard from '../../components/AvatarCard.vue';
    export default {
        name: 'AvatarGallery',
        components: { AvatarCard },
        inject: ['API'],
        props: {
            menuActiveIndex: String,
            galleryAvatars: Array
        },
        data() {
            return {
                selectedTags: [],
                searchQuery: ''
            };
        },
        computed: {
            allTags() {
                const set = new Set();
                for (const a of this.galleryAvatars) {
                    if (a.tags) {
                        a.tags.forEach((t) => set.add(t));
                    }
                }
                return Array.from(set);
            },
            filteredAvatars() {
                const tags = Array.isArray(this.selectedTags)
                    ? this.selectedTags
                    : [];
                const q = this.searchQuery.toLowerCase().trim();
                return this.galleryAvatars.filter((a) => {
                    if (
                        tags.length &&
                        !tags.every((tag) => a.tags && a.tags.includes(tag))
                    ) {
                        return false;
                    }
                    if (
                        q &&
                        !(a.name && a.name.toLowerCase().includes(q))
                    ) {
                        return false;
                    }
                    return true;
                });
            }
        }
    };
</script>

<style scoped>
    .avatar-gallery {
        display: flex;
        flex-wrap: wrap;
        gap: 10px;
        justify-content: center;
    }
</style>
